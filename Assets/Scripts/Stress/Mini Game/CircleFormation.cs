using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    [CreateAssetMenu(menuName = "Eternal Return/Formations/Circle")]
    public class CircleFormation : Formation
    {
        [Min(0f)]
        public float Radius = 1f;

        public override bool IsClosed => true;

        protected override Vector2 GetLocalPoint(float t)
        {
            var angle = t * Mathf.PI * 2f;
            return new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * Radius;
        }
    }
}
