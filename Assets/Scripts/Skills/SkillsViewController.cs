using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsViewController : ITickable, IStartable
    {
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

                if (skillSocket.IsFilled)
                {
                    var skillView = Object.Instantiate(_skillViewPrefab, skillSocketView.gameObject.transform);
                    skillView.SetSkill(skillSocket.Skill);
                    skillView.SkillsViewController = this;
                    
                    skillSocketView.SkillView = skillView;
                }
            }
        }

        public void Tick()
        {
            if (!_isDragging) return;
            var pointerPosition = Pointer.current.position.value;
            _draggingSkillView.transform.position = new Vector3(pointerPosition.x, pointerPosition.y);
        }

        public void StartDrag(SkillView skillView)
        {
            _draggingSkillView = skillView;
            _isDragging = true;
        }

        public void StopDrag()
        {
            _isDragging = false;
            _draggingSkillView = null;
        }
    }
}