using System;
using UnityEngine;

namespace EternalReturn.Stress
{
    [Serializable]
    public class Transform2D
    {
        public Vector2 Position;
        public float Rotation;
        public Vector2 Scale = Vector2.one;

        public Vector2 TransformPoint(Vector2 point)
        {
            var scaled = Vector2.Scale(point, Scale);
            var rotated = (Vector2)(Quaternion.Euler(0f, 0f, Rotation) * scaled);
            return Position + rotated;
        }
    }
}
