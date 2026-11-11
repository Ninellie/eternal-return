using UnityEngine;

namespace EternalReturn.Stress
{
    public static class CollisionPrimitives
    {
        public static Polygon[] Triangle()
        {
            return new[]
            {
                Create(new Vector2(-0.5f, -0.5f), new Vector2(0.5f, -0.5f), new Vector2(0f, 0.5f)),
            };
        }

        public static Polygon[] Heart()
        {
            return new[]
            {
                Create(new Vector2(-0.5f, 0.2f), new Vector2(0f, -0.5f), new Vector2(0f, 0.1f)),
                Create(new Vector2(-0.5f, 0.2f), new Vector2(0f, 0.1f), new Vector2(0f, 0.25f)),
                Create(new Vector2(-0.5f, 0.2f), new Vector2(-0.25f, 0.5f), new Vector2(0f, 0.25f)),
                Create(new Vector2(0.5f, 0.2f), new Vector2(0f, -0.5f), new Vector2(0f, 0.1f)),
                Create(new Vector2(0.5f, 0.2f), new Vector2(0f, 0.1f), new Vector2(0f, 0.25f)),
                Create(new Vector2(0.5f, 0.2f), new Vector2(0.25f, 0.5f), new Vector2(0f, 0.25f)),
            };
        }

        public static Polygon[] Bullet()
        {
            return new[]
            {
                Create(new Vector2(-0.25f, -0.5f), new Vector2(0.25f, -0.5f), new Vector2(0.25f, 0.1f)),
                Create(new Vector2(-0.25f, -0.5f), new Vector2(0.25f, 0.1f), new Vector2(-0.25f, 0.1f)),
                Create(new Vector2(-0.25f, 0.1f), new Vector2(0.25f, 0.1f), new Vector2(0f, 0.5f)),
            };
        }

        private static Polygon Create(Vector2 a, Vector2 b, Vector2 c)
        {
            return new Polygon { A = a, B = b, C = c };
        }
    }
}
