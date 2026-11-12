using EternalReturn.Stress.Collision_System;
using EternalReturn.Stress.Movement_System;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    public class Enemy : IColliding, IMovable
    {
        public float WaitTime { get; set; }

        public Vector2 Direction { get; set; }

        public float Speed { get; set; }

        public Transform2D Transform { get; } = new();

        public Mesh2D Mesh { get; set; }

        public CollisionLayer Layer => CollisionLayer.Enemy;

        public CollisionLayer CollidesWith => CollisionLayer.Player;

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
