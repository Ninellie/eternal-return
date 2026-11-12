using UnityEngine;

namespace EternalReturn.Stress.Movement_System
{
    public interface IMovable
    {
        Vector2 Direction { get; }

        float Speed { get; }

        Transform2D Transform { get; }
    }
}
