using System.Collections.Generic;
using UnityEngine;

namespace EternalReturn.Skills
{
    public class SkillSocketOverlapHelper
    {
        private readonly Vector3[] _worldCorners = new Vector3[4];
        private readonly IEnumerable<SkillSocketView> _skillSocketViews;

        public SkillSocketOverlapHelper(IEnumerable<SkillSocketView> skillSocketViews)
        {
            _skillSocketViews = skillSocketViews;
        }
        
        public bool TryGetClosestOverlappedSocket(RectTransform draggingSkillView, out SkillSocketView socketView)
        {
            var draggingRect = GetWorldRect(draggingSkillView);
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
    }
}