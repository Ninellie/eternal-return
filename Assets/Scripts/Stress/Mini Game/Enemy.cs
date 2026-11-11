using UnityEngine;

namespace EternalReturn.Stress
{
    public class Enemy : IColliding, IMovable
    {
        public Vector2 Velocity { get; set; }

        public Vector2 Position { get; set; }

        public Vector2 Scale { get; set; } = Vector2.one;

        public float Rotation { get; set; }

        public Polygon[] Polygons { get; set; }

        public void OnCollisionEnter(IColliding other)
        {
        }

        public void OnCollisionStay(IColliding other)
        {
        }

        public void OnCollisionExit(IColliding other)
        {
        }
    }
}
