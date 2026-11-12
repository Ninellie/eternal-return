using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    [CreateAssetMenu(menuName = "Eternal Return/Formations/Box")]
    public class BoxFormation : Formation
    {
        [Tooltip("Ширина и высота коробки в локальных координатах. Центр коробки - Position.")]
        public Vector2 Size = Vector2.one;

        public override bool IsClosed => true;

        public bool Contains(Vector2 point)
        {
            var scale = Transform.Scale;
            if (Mathf.Approximately(scale.x, 0f) || Mathf.Approximately(scale.y, 0f)) return false;

            var offset = point - Transform.Position;
            var unrotated = (Vector2)(Quaternion.Euler(0f, 0f, -Transform.Rotation) * offset);
            var local = new Vector2(unrotated.x / scale.x, unrotated.y / scale.y);
            var half = Size * 0.5f;

            return Mathf.Abs(local.x) <= half.x && Mathf.Abs(local.y) <= half.y;
        }

        protected override Vector2 GetLocalPoint(float t)
        {
            var half = Size * 0.5f;
            var corners = new[]
            {
                new Vector2(-half.x, -half.y),
                new Vector2(-half.x, half.y),
                new Vector2(half.x, half.y),
                new Vector2(half.x, -half.y),
            };

            return GetPointOnPath(corners, true, t);
        }
    }
}
