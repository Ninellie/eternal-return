using System.Collections.Generic;
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
        
        private SkillView _pressedSkillView;
        private Vector2 _pressPosition;
        private SkillView _draggingSkillView;
        private bool _isDragging;
        private readonly Vector3[] _worldCorners = new Vector3[4];
        
        private SkillSocketView _lastHighlightedSocketView;
        
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
                skillView.SetSkillSocketView(skillSocketView);
                skillView.SkillsViewController = this;
                    
                skillSocketView.SkillView = skillView;
            }
        }

        public void Tick()
        {
            if (_pressedSkillView == null && !_isDragging) return;

            var pointerPosition = Pointer.current.position.value;
            TryStartDrag(pointerPosition);

            if (!_isDragging) return;

            _draggingSkillContainer.transform.position = new Vector3(pointerPosition.x, pointerPosition.y);

            if (!TryGetClosestOverlappedSocket(out var socketView))
            {
                if (_lastHighlightedSocketView != null)
                {
                    _lastHighlightedSocketView.SetNormal();
                }

                _lastHighlightedSocketView = null;
                
                return;
            }

            if (socketView.SkillSocket.IsLocked) return;
            
            socketView.SetHighlighted();
            _lastHighlightedSocketView = socketView;
                
            foreach (var skillSocketView in _skillSocketViews)
            {
                if (skillSocketView == socketView) continue;
                if (skillSocketView.SkillSocket.IsLocked) continue;
                    
                skillSocketView.SetNormal();
            }
        }

        private bool TryGetClosestOverlappedSocket(out SkillSocketView socketView)
        {
            var draggingRect = GetWorldRect((RectTransform)_draggingSkillView.transform);
            socketView = null;
            var closestDistance = float.MaxValue;

            foreach (var skillSocketView in _skillSocketViews)
            {
                var socketRect = GetWorldRect((RectTransform)skillSocketView.transform);
                if (!draggingRect.Overlaps(socketRect)) continue;

                var distance = Vector2.Distance(draggingRect.center, socketRect.center);
                if (distance >= closestDistance) continue;

                closestDistance = distance;
                socketView = skillSocketView;
            }

            return socketView != null;
        }

        private Rect GetWorldRect(RectTransform rectTransform)
        {
            rectTransform.GetWorldCorners(_worldCorners);
            var bottomLeft = _worldCorners[0];
            var topRight = _worldCorners[2];
            return Rect.MinMaxRect(bottomLeft.x, bottomLeft.y, topRight.x, topRight.y);
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

        public void Press(SkillView skillView, Vector2 pressPosition)
        {
            _pressedSkillView = skillView;
            _pressPosition = pressPosition;
        }

        public void Release()
        {
            _pressedSkillView = null;
            EndDrag();
        }

        private void TryStartDrag(Vector2 pointerPosition)
        {
            if (_pressedSkillView == null || _isDragging) return;
            if (Vector2.Distance(pointerPosition, _pressPosition) < DragThresholdPixels) return;

            StartDrag(_pressedSkillView);
        }

        private void StartDrag(SkillView skillView)
        {
            _draggingSkillView = skillView;
            _isDragging = true;
            
            _draggingSkillContainer.transform.position = skillView.transform.position;
            skillView.transform.SetParent(_draggingSkillContainer);
        }

        private void EndDrag()
        {
            _draggingSkillView.transform.SetParent(_draggingSkillView.Socket.transform);
            _draggingSkillView.transform.position = _draggingSkillView.Socket.transform.position;
            
            _lastHighlightedSocketView.SetNormal();
            _lastHighlightedSocketView = null;
            _isDragging = false;
            _draggingSkillView = null;
        }
    }
}