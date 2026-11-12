using System;
using EternalReturn.Stress.Collision_System;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    public class Player : IColliding
    {
        public Transform2D Transform { get; } = new();

        public Mesh2D Mesh { get; }

        public CollisionLayer Layer => CollisionLayer.Player;

        public CollisionLayer CollidesWith => CollisionLayer.Enemy;

        public int Health { get; private set; }

        public int TakenDamage { get; }

        private readonly StressGameController _controller;

        public Player(StressGameController controller, StressGameConfig config)
        {
            if (config.PlayerShape == null)
            {
                throw new InvalidOperationException("StressGameConfig has no player shape.");
            }

            _controller = controller;
            Transform.Scale = config.PlayerScale;
            Mesh = config.PlayerShape.Mesh;
            Health = config.PlayerStartHealth;
            TakenDamage = config.PlayerDamageTaken;
        }

        public void OnCollisionEnter(IColliding other)
        {
            Health = Mathf.Max(Health - TakenDamage, 0);
            if (Health > 0) return;

            _controller.LoseGame();
        }

        public void OnCollisionStay(IColliding other)
        {
        }

        public void OnCollisionExit(IColliding other)
        {
        }
    }
}
