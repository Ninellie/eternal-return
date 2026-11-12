using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    public abstract class Formation : ScriptableObject
    {
        public Transform2D Transform = new();

        public abstract bool IsClosed { get; }

        public Vector2 GetDistributedPoint(int index, int count)
        {
            if (count <= 1) return GetPoint(0.5f);

            var step = IsClosed ? count : count - 1;
            return GetPoint(index / (float)step);
        }

        public Vector2 GetPoint(float t)
        {
            var scaled = Vector2.Scale(GetLocalPoint(Mathf.Clamp01(t)), Transform.Scale);
            var rotated = (Vector2)(Quaternion.Euler(0f, 0f, Transform.Rotation) * scaled);
            return Transform.Position + rotated;
        }

        protected abstract Vector2 GetLocalPoint(float t);

        protected static Vector2 GetPointOnPath(Vector2[] points, bool isClosed, float t)
        {
            var segmentsAmount = isClosed ? points.Length : points.Length - 1;
            var totalLength = 0f;
            for (var i = 0; i < segmentsAmount; i++)
            {
                totalLength += Vector2.Distance(points[i], points[(i + 1) % points.Length]);
            }

            if (totalLength <= 0f) return points[0];

            var remaining = t * totalLength;
            for (var i = 0; i < segmentsAmount; i++)
            {
                var start = points[i];
                var end = points[(i + 1) % points.Length];
                var length = Vector2.Distance(start, end);
                if (remaining > length)
                {
                    remaining -= length;
                    continue;
                }

                return length > 0f ? Vector2.Lerp(start, end, remaining / length) : start;
            }

            return points[isClosed ? 0 : points.Length - 1];
        }
    }
}
