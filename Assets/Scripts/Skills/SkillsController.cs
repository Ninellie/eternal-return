using System;
using System.Collections.Generic;
using System.Linq;
using EternalReturn.Burnout;
using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsController : IStartable, IFixedTickable, ISlowableByBurnout
    {
        private const float Slowdown = 0.05f;
        private const int SocketIntelPrice = 10;

        /// <summary>
        /// Вызывается после того как умение заняло сокет.
        /// </summary>
        public event Action<SkillSocket> OnSocketOccupied;

        /// <summary>
        /// Вызывается после того как был открыт новый сокет.
        /// </summary>
        public event Action<SkillSocket> OnSocketCreated;

        public List<SkillSocket> Sockets { get; } = new();

        public bool IsSlowed { get; private set; }

        private readonly Resource _intel;
        private readonly Resource _burnout;

        public SkillsController(ResourceProvider resourceProvider)
        {
            _intel = resourceProvider.Intel;
            _burnout = resourceProvider.Burnout;
        }

        public void Start()
        {
            _burnout.OnFill += Slow;
            _burnout.OnDecrease += _ => Unslow();
        }

        public void FixedTick()
        {
            var deltaTime = Time.deltaTime;

            if (IsSlowed)
            {
                deltaTime *= Slowdown;
            }

            // Обработка кулдаунов тем и если кулдаун истёк, пополнение ресурса знаний
            foreach (var socket in Sockets)
            {
                if (!socket.IsOccupied) continue;
                
                var skill = socket.Skill;
                
                skill.Cooldown -= deltaTime;

                if (skill.Cooldown > 0) continue;
            
                skill.Cooldown = skill.BaseCooldown;
                
                _intel.Increase(skill.IntelGain);
            }
        }

        public void Slow()
        {
            IsSlowed = true;
        }

        public void Unslow()
        {
            IsSlowed = false;
        }

        public void CreateSocket()
        {
            var price = Sockets.Count == 0 ? 0 : SocketIntelPrice;
            
            if (price > _intel.Amount) return;
            
            _intel.Decrease(SocketIntelPrice);
            
            var slot = new SkillSocket();
            Sockets.Add(slot);
            OnSocketCreated?.Invoke(slot);
        }

        public void InsertSkill(Skill value)
        {
            var hasEmptySockets = Sockets.Any(s => !s.IsOccupied);
            
            if (!hasEmptySockets) return;
            
            var emptySocket = Sockets.First(s => !s.IsOccupied);

            emptySocket.Skill = value;
            emptySocket.IsOccupied = true;
            
            OnSocketOccupied?.Invoke(emptySocket);
        }
    }
}