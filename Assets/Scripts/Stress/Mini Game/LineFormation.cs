using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    [CreateAssetMenu(menuName = "Eternal Return/Formations/Line")]
    public class LineFormation : Formation
    {
        [Tooltip("Начало линии в локальных координатах.")]
        public Vector2 Start = new(-0.5f, 0f);

        [Tooltip("Конец линии в локальных координатах.")]
        public Vector2 End = new(0.5f, 0f);

        public override bool IsClosed => false;

        protected override Vector2 GetLocalPoint(float t)
        {
            return Vector2.Lerp(Start, End, t);
        }
    }
}
