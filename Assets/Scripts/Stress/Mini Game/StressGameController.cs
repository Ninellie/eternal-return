using System;
using System.Collections.Generic;
using EternalReturn.Resources_Feature;
using EternalReturn.Stress.Collision_System;
using EternalReturn.Stress.Movement_System;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress.Mini_Game
{
    public class StressGameController : IStartable, IFixedTickable
    {
        public event Action OnGameLost;
        public event Action OnGameWon;

        private readonly StressGameConfig _config;
        private readonly CollisionSystem _collisionSystem;
        private readonly MovementSystem _movementSystem;
        private readonly Resource _stress;
        private readonly List<Enemy> _enemies = new();

        private Scenario _scenario;
        private Player _player;

        private bool _isInProcess;

        public StressGameController(
            StressGameConfig config,
            CollisionSystem collisionSystem,
            MovementSystem movementSystem,
            ResourceProvider resourceProvider)
        {
            _config = config;
            _collisionSystem = collisionSystem;
            _movementSystem = movementSystem;
            _stress = resourceProvider.Stress;
        }

        public void Start()
        {
            _stress.OnFill += StartGame;
        }

        public void StartGame()
        {
            _scenario = _config.Roll();
            _player = new Player(this, _config);
            _collisionSystem.Register(_player);
            _isInProcess = true;
        }

        public void LoseGame()
        {
            if (!_isInProcess) return;

            _isInProcess = false;
            Clear();
            OnGameLost?.Invoke();
        }

        public void WinGame()
        {
            if (!_isInProcess) return;

            _isInProcess = false;
            Clear();
            OnGameWon?.Invoke();
        }

        public void FixedTick()
        {
            if (!_isInProcess) return;

            TickEnemies();
            RemoveEnemiesOutOfField();
            TickSpawns();
            TickScenarioTime();
        }

        private void TickEnemies()
        {
            foreach (var enemy in _enemies)
            {
                if (enemy.WaitTime <= 0f) continue;

                enemy.WaitTime -= Time.fixedDeltaTime;
                if (enemy.WaitTime > 0f) continue;

                Activate(enemy);
            }
        }

        private void RemoveEnemiesOutOfField()
        {
            for (var i = _enemies.Count - 1; i >= 0; i--)
            {
                var enemy = _enemies[i];
                if (enemy.WaitTime > 0f) continue;
                if (_config.GameField.Contains(enemy.Transform.Position)) continue;

                _enemies.RemoveAt(i);
                _collisionSystem.Unregister(enemy);
                _movementSystem.Unregister(enemy);
            }
        }

        private void TickSpawns()
        {
            _scenario.TimeToNextSpawn -= Time.fixedDeltaTime;
            if (_scenario.TimeToNextSpawn > 0) return;
            if (!_scenario.Spawns.TryDequeue(out var current)) return;

            Spawn(current.Spawn);

            if (_scenario.Spawns.TryPeek(out var next))
            {
                _scenario.TimeToNextSpawn = next.Interval;
            }
        }

        private void TickScenarioTime()
        {
            _scenario.TimeLeft -= Time.fixedDeltaTime;
            if (_scenario.TimeLeft >= 0f) return;

            WinGame();
        }

        private void Spawn(Spawn spawn)
        {
            foreach (var enemy in spawn.Enemies)
            {
                enemy.Direction = (_player.Transform.Position - enemy.Transform.Position).normalized;
                _enemies.Add(enemy);
                if (enemy.WaitTime > 0f) continue;

                Activate(enemy);
            }
        }

        private void Activate(Enemy enemy)
        {
            _collisionSystem.Register(enemy);
            _movementSystem.Register(enemy);
        }

        private void Clear()
        {
            foreach (var enemy in _enemies)
            {
                _collisionSystem.Unregister(enemy);
                _movementSystem.Unregister(enemy);
            }

            _enemies.Clear();
            _collisionSystem.Unregister(_player);
            _scenario = null;
            _player = null;
        }
    }
}
