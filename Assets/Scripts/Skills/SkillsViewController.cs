using UnityEngine;
using UnityEngine.InputSystem;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsViewController : ITickable
    {
        private SkillSocketView[] _skillSocketViews;

        private SkillView _draggingSkillView;
        private bool _isDragging;
        
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

        public void Tick()
        {
            if (!_isDragging) return;
            var pointerPosition = Pointer.current.position.value;
            _draggingSkillView.transform.position = new Vector3(pointerPosition.x, pointerPosition.y);
        }
    }
}