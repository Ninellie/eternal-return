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
    }
}
