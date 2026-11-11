using UnityEngine;

namespace EternalReturn.Stress
{
    public interface IColliding
    {
        Vector2 Position { get; }

        Vector2 Scale { get; }

        float Rotation { get; }

        Polygon[] Polygons { get; }

        void OnCollisionEnter(IColliding other);
        void OnCollisionStay(IColliding other);
        void OnCollisionExit(IColliding other);
    }
}
