using System;

namespace EternalReturn.Stress.Collision_System
{
    [Flags]
    public enum CollisionLayer
    {
        None = 0,
        Player = 1,
        Enemy = 2,
    }
}
