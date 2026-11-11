using UnityEngine;

namespace EternalReturn.Stress
{
    public interface IMovable
    {
        Vector2 Velocity { get; }

        Vector2 Position { get; set; }
    }
}
