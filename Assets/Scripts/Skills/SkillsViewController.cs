using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsViewController : ITickable, IStartable
    {
        private const float DragThresholdPixels = 24f;

        private SkillView _pressedSkillView;
        private Vector2 _pressPosition;
        private SkillView _draggingSkillView;
        private bool _isDragging;
        
        private readonly Transform _skillsPanel;
        private readonly SkillView _skillViewPrefab;
        private readonly SkillSocketView _skillSocketViewPrefab;
        private readonly SkillsController _skillsController;
        private readonly List<SkillSocketView> _skillSocketViews;

        public SkillsViewController([Key("skills-panel")] Transform skillsPanel,
            SkillView skillViewPrefab,
            SkillSocketView skillSocketViewPrefab,
            SkillsController skillsController)
        {
            _skillsPanel = skillsPanel;
            _skillViewPrefab = skillViewPrefab;
            _skillSocketViewPrefab = skillSocketViewPrefab;
            _skillsController = skillsController;
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

            _draggingSkillView.transform.position = new Vector3(pointerPosition.x, pointerPosition.y);
        }

        public void Press(SkillView skillView, Vector2 pressPosition)
        {
            _pressedSkillView = skillView;
            _pressPosition = pressPosition;
        }

        public void Release()
        {
            _pressedSkillView = null;
            StopDrag();
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
        }

        private void StopDrag()
        {
            _isDragging = false;
            _draggingSkillView = null;
        }
    }
}