using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress.Mini_Game
{
    public class PlayerMovementController : IStartable, IFixedTickable
    {
        private readonly InputManager _inputManager;
        private readonly StressGameConfig _config;

        private Player _player;
        private Vector2 _direction;

        public PlayerMovementController(InputManager inputManager, StressGameConfig config)
        {
            _inputManager = inputManager;
            _config = config;
        }

        public void Start()
        {
            _inputManager.OnMove += SetDirection;
        }

        public void Register(Player player)
        {
            _player = player;
        }

        public void Unregister()
        {
            _player = null;
            _direction = Vector2.zero;
        }

        public void FixedTick()
        {
            if (_player == null) return;

            var transform = _player.Transform;
            transform.Position += _direction * (_config.PlayerSpeed * Time.fixedDeltaTime);
            transform.Position += BoxBoundary.GetPushInsideOffset(_config.PlayerField, transform, _player.Mesh);
        }

        private void SetDirection(Vector2 direction)
        {
            _direction = direction.normalized;
        }
    }
}
