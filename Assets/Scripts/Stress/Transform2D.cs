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

        public bool HasValidScale => !Mathf.Approximately(Scale.x, 0f) && !Mathf.Approximately(Scale.y, 0f);

        public Vector2 TransformPoint(Vector2 point)
        {
            return Position + TransformVector(point);
        }

        public Vector2 TransformVector(Vector2 vector)
        {
            var scaled = Vector2.Scale(vector, Scale);
            return (Vector2)(Quaternion.Euler(0f, 0f, Rotation) * scaled);
        }

        public Vector2 InverseTransformPoint(Vector2 point)
        {
            var unrotated = (Vector2)(Quaternion.Euler(0f, 0f, -Rotation) * (point - Position));
            return new Vector2(unrotated.x / Scale.x, unrotated.y / Scale.y);
        }
    }
}
