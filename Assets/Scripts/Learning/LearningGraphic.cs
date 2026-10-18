using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Learning
{
    public class LearningGraphic : MaskableGraphic
    {
        private const int Segments = 32;

        [SerializeField] private Color outerColor = new(1f, 1f, 1f, 0.3f);
        [SerializeField] private Color durationColor = Color.yellow;

        private LearningProcess _process;
        private bool _isLearning;

        public void Stop()
        {
            _isLearning = false;
            _process = null;
            SetVerticesDirty();
        }
        
        public void SetLearningProcess(LearningProcess process)
        {
            _process = process;
            _isLearning = true;
            SetVerticesDirty();
        }

        private void Update()
        {
            if (!_isLearning) return;
            SetVerticesDirty();
        }

        public bool IsHit(LearningNode node, Vector2 clickPosition)
        {
            if (!_isLearning) return false;

            var eventCamera = canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera;
            var isInside = RectTransformUtility.ScreenPointToLocalPointInRectangle(
                rectTransform, clickPosition, eventCamera, out var localPoint);
            if (!isInside) return false;

            var radius = _process.NodeRadius;
            var center = GetNodeCenter(node, GetPixelAdjustedRect(), radius);
            return (localPoint - center).sqrMagnitude <= radius * radius;
        }

        private static Vector2 GetNodeCenter(LearningNode node, Rect area, float radius)
        {
            var width = Mathf.Max(area.width - 2f * radius, 0f);
            var height = Mathf.Max(area.height - 2f * radius, 0f);

            return new Vector2(
                area.xMin + radius + node.Position.x * width,
                area.yMin + radius + node.Position.y * height);
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (!_isLearning) return;

            var area = GetPixelAdjustedRect();
            var radius = _process.NodeRadius;

            foreach (var node in _process.CurrentNodes)
            {
                var center = GetNodeCenter(node, area, radius);
                
                AddCircle(vh, center, radius, outerColor);

                var durationRatio = node.BaseDuration > 0f
                    ? Mathf.Clamp01(node.Duration / node.BaseDuration)
                    : 0f;
                
                AddCircle(vh, center, radius * durationRatio, durationColor);
            }
        }

        private static void AddCircle(VertexHelper vh, Vector2 center, float radius, Color circleColor)
        {
            if (radius <= 0f) return;

            var centerIndex = vh.currentVertCount;
            vh.AddVert(center, circleColor, Vector2.zero);

            for (var i = 0; i < Segments; i++)
            {
                var angle = 2f * Mathf.PI * i / Segments;
                var offset = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius;
                vh.AddVert(center + offset, circleColor, Vector2.zero);
            }

            for (var i = 0; i < Segments; i++)
            {
                var current = centerIndex + 1 + i;
                var next = centerIndex + 1 + (i + 1) % Segments;
                vh.AddTriangle(centerIndex, next, current);
            }
        }
    }
}
