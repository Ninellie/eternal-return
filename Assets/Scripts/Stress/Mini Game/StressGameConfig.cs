using System;
using System.Collections.Generic;
using EternalReturn.Stress.Collision_System;
using UnityEngine;

namespace EternalReturn.Stress.Mini_Game
{
    [CreateAssetMenu(menuName = "Eternal Return/Stress Game Config")]
    public class StressGameConfig : ScriptableObject
    {
        [Header("Игровое поле")]
        [Tooltip("Границы игрового поля.")]
        public BoxFormation GameField;

        [Tooltip("Границы поля игрока.")]
        public BoxFormation PlayerField;

        [Header("Игрок")]
        [Min(1)]
        [Tooltip("Начальное здоровье игрока.")]
        public int PlayerStartHealth = 10;

        [Min(0)]
        [Tooltip("Урон, который получает игрок при столкновении с врагом.")]
        public int PlayerDamageTaken = 1;

        [Tooltip("Форма игрока.")]
        public Shape2D PlayerShape;

        [Tooltip("Размер игрока в пикселях. Применяется как масштаб к единичной форме.")]
        public Vector2 PlayerScale = new(40f, 40f);

        [Header("Сценарий")]
        [Min(0.1f)]
        [Tooltip("Длительность сценария в секундах.")]
        public float Duration = 30f;

        [Min(0f)]
        [Tooltip("Отступ от начала сценария в секундах. Спавны начинают распределяться после него.")]
        public float SpawnStartOffset = 1f;

        [Min(0f)]
        [Tooltip("Отступ от конца сценария в секундах. Спавны заканчиваются за это время до конца.")]
        public float SpawnEndOffset = 3f;

        [Tooltip("Построение, по периметру которого размещаются враги.")]
        public Formation Formation;

        [Min(1)]
        [Tooltip("Количество спавнов за время сценария.")]
        public int SpawnsAmount = 10;

        [Min(1)]
        [Tooltip("Количество врагов в одном спавне.")]
        public int EnemiesAmount = 5;

        [Tooltip("Если включено, каждому врагу выбирается случайная форма из примитивов. Иначе используется форма из поля EnemyShape.")]
        public bool UseRandomShape;

        [Tooltip("Форма врага.")]
        public Shape2D EnemyShape;

        [Tooltip("Размер врага в пикселях. Применяется как масштаб к единичной форме.")]
        public Vector2 EnemyScale = new(30f, 30f);

        [Min(0f)]
        [Tooltip("Скорость врага.")]
        public float EnemySpeed = 1f;

        [Min(0f)]
        [Tooltip("Время ожидания врага в секундах. Враг уже появился и виден на экране, но стоит на месте и начинает движение только после окончания этого времени.")]
        public float EnemyWaitTime = 1f;

        public Scenario Roll()
        {
            if (Formation == null)
            {
                throw new InvalidOperationException("StressGameConfig has no formation.");
            }

            if (!UseRandomShape && EnemyShape == null)
            {
                throw new InvalidOperationException("StressGameConfig has no enemy shape.");
            }

            var spawns = new Queue<(float Interval, Spawn Spawn)>();
            var spawnsDuration = Mathf.Max(Duration - SpawnStartOffset - SpawnEndOffset, 0f);
            var interval = spawnsDuration / SpawnsAmount;

            for (var i = 0; i < SpawnsAmount; i++)
            {
                var delay = i == 0 ? SpawnStartOffset + interval : interval;
                spawns.Enqueue((delay, CreateSpawn()));
            }

            return new Scenario(spawns, Duration);
        }

        private Spawn CreateSpawn()
        {
            var enemies = new Enemy[EnemiesAmount];
            for (var i = 0; i < enemies.Length; i++)
            {
                enemies[i] = CreateEnemy(Formation.GetDistributedPoint(i, enemies.Length));
            }

            return new Spawn(enemies);
        }

        private Enemy CreateEnemy(Vector2 position)
        {
            var enemy = new Enemy
            {
                Mesh = UseRandomShape ? CollisionPrimitives.CreateRandom() : EnemyShape.Mesh,
                Speed = EnemySpeed,
                WaitTime = EnemyWaitTime,
            };

            enemy.Transform.Position = position;
            enemy.Transform.Scale = EnemyScale;
            return enemy;
        }
    }
}
