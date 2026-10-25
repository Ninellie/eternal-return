using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsViewController : ITickable, IStartable
    {
        private const float DragThresholdPixels = 28f;
        
        private readonly Transform _skillsPanel;
        private readonly Transform _draggingSkillContainer;
        private readonly SkillView _skillViewPrefab;
        private readonly SkillSocketView _skillSocketViewPrefab;
        private readonly SkillsController _skillsController;
        private readonly List<SkillSocketView> _skillSocketViews;
        private readonly SkillSocketOverlapHelper _overlapHelper;
        
        private SkillView _pressedSkillView;
        private Vector2 _pressPosition;
        private SkillView _draggingSkillView;
        private bool _isDragging;
        
        private SkillSocketView _currentEmptyOverlappedSocketView;
        
        public SkillsViewController([Key("skills-panel")] Transform skillsPanel,
            SkillView skillViewPrefab,
            SkillSocketView skillSocketViewPrefab,
            SkillsController skillsController,
            [Key("skill-container")] Transform draggingSkillContainer)
        {
            _skillsPanel = skillsPanel;
            _skillViewPrefab = skillViewPrefab;
            _skillSocketViewPrefab = skillSocketViewPrefab;
            _skillsController = skillsController;
            _draggingSkillContainer = draggingSkillContainer;
            _skillSocketViews = new List<SkillSocketView>();
            _overlapHelper = new SkillSocketOverlapHelper(_skillSocketViews);
        }

        public void Start()
        {
            foreach (var skillSocket in _skillsController.SkillSockets)
            {
                var skillSocketView = Object.Instantiate(_skillSocketViewPrefab, _skillsPanel);
                _skillSocketViews.Add(skillSocketView);
                
                skillSocketView.SetSocket(skillSocket);
                
                if (!skillSocket.IsFilled) continue;
                
                var skillView = Object.Instantiate(_skillViewPrefab, skillSocketView.gameObject.transform);
                skillView.SetSkill(skillSocket.Skill);
                skillView.SkillsViewController = this;
                
                skillSocketView.SetSkillView(skillView);
            }
        }

        public void StartDrag(SkillView skillView, Vector2 pressPosition)
        {
            _pressedSkillView = skillView;
            _pressPosition = pressPosition;
        }

        private void TryStartDrag(Vector2 pointerPosition)
        {
            if (_pressedSkillView == null || _isDragging) return;
            if (Vector2.Distance(pointerPosition, _pressPosition) < DragThresholdPixels) return;
            _draggingSkillView = _pressedSkillView;
            _isDragging = true;
            
            _draggingSkillContainer.transform.position = _pressedSkillView.transform.position;
            _pressedSkillView.transform.SetParent(_draggingSkillContainer);
        }

        public void Tick()
        {
            if (_pressedSkillView == null && !_isDragging) return;

            var pointerPosition = Pointer.current.position.value;
            TryStartDrag(pointerPosition);

            if (!_isDragging) return;

            _draggingSkillContainer.transform.position = new Vector3(pointerPosition.x, pointerPosition.y);

            var draggingRectTransform = _draggingSkillView.RectTransform;
            
            var isOverlapping = _overlapHelper.TryGetClosestOverlappedSocket(draggingRectTransform, out var socketView); 
            
            if (!isOverlapping)
            {
                _currentEmptyOverlappedSocketView?.SetNormal();
                _currentEmptyOverlappedSocketView = null;
                return;
            }
            
            var isLocked = socketView.SkillSocket.IsLocked;
            
            if (isLocked)
            {
                _currentEmptyOverlappedSocketView?.SetNormal();
                _currentEmptyOverlappedSocketView = null;
                return;
            }
            
            socketView.SetHighlighted();
            _currentEmptyOverlappedSocketView = socketView;
                
            foreach (var skillSocketView in _skillSocketViews)
            {
                if (skillSocketView == socketView) continue;
                if (skillSocketView.SkillSocket.IsLocked) continue;
                    
                skillSocketView.SetNormal();
            }
        }

        public void Hover(SkillView skillView)
        {
            var discipline = skillView.Skill.Discipline;
            var tier = skillView.Skill.Tier;
            
            foreach (var skillSocketView in _skillSocketViews)
            {
                if (!skillSocketView.SkillSocket.IsFilled) continue;
                
                var skill = skillSocketView.SkillView.Skill;

                var isSameDiscipline = string.Equals(discipline, skill.Discipline);

                if (!isSameDiscipline)
                {
                    skillSocketView.SkillView.SetDisabled();
                    continue;
                }

                if (tier.Equals(skill.Tier))
                {
                    skillSocketView.SkillView.SetHighlighted();
                    continue;
                }
                
                skillSocketView.SkillView.SetNormal();
            }
        }

        public void EndHover()
        {
            foreach (var skillSocketView in _skillSocketViews)
            {
                if (!skillSocketView.SkillSocket.IsFilled) continue;
                skillSocketView.SkillView.SetNormal();
            }
        }

        public void EndDrag()
        {
            _pressedSkillView = null;

            if (!_isDragging) return; 
            if (_currentEmptyOverlappedSocketView != null)
            {
                _currentEmptyOverlappedSocketView.SetNormal();
                
                var draggedSkill = _draggingSkillView.Skill;
                var targetSocket = _currentEmptyOverlappedSocketView.SkillSocket;

                if (targetSocket.IsFilled && _skillsController.CanMerge(targetSocket.Skill, draggedSkill))
                {
                    var sourceSocketView = _skillSocketViews.First(x => x.SkillView == _draggingSkillView);
                    var survivorView = _currentEmptyOverlappedSocketView.SkillView;

                    _skillsController.Merge(targetSocket.Skill, draggedSkill);

                    sourceSocketView.EmptySkillView();
                    Object.Destroy(_draggingSkillView.gameObject);
                    survivorView.SetSkill(targetSocket.Skill);

                    _currentEmptyOverlappedSocketView = null;
                    _isDragging = false;
                    _draggingSkillView = null;
                    return;
                }

                var isFilled = targetSocket.IsFilled;

                _skillsController.FillSocketWithSkill(draggedSkill, targetSocket);
                
                // Найти вью сокета где до этого был скилл
                var otherSocketView = _skillSocketViews.First(x => x.SkillView == _draggingSkillView);
                
                if (!isFilled)
                {
                    otherSocketView.EmptySkillView();
                }
                else
                {
                    // Найти другой скилл
                    var otherSkillView = _currentEmptyOverlappedSocketView.SkillView;
                    
                    // Установить в прошлый сокет другой скилл
                    otherSocketView.SetSkillView(otherSkillView);
                    
                    otherSkillView.transform.SetParent(otherSocketView.transform);
                    otherSkillView.transform.position = otherSocketView.transform.position;
                }
                
                // Установить в таргетный сокет новый скилл
                _currentEmptyOverlappedSocketView.SetSkillView(_draggingSkillView);
            }

            var socketView = _skillSocketViews.First(x => x.SkillView == _draggingSkillView);
            var socketTransform = socketView.transform;
            
            _draggingSkillView.transform.SetParent(socketTransform);
            _draggingSkillView.transform.position = socketTransform.position;
            
            _currentEmptyOverlappedSocketView = null;
            _isDragging = false;
            _draggingSkillView = null;
        }
    }
}