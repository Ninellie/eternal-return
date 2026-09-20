using System;
using System.Collections.Generic;
using System.Linq;
using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsController : IFixedTickable
    {
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

        private readonly Resource _intel;

        public SkillsController(ResourceProvider resourceProvider)
        {
            _intel = resourceProvider.Intel;
        }

        public void FixedTick()
        {
            // Обработка кулдаунов тем и если кулдаун истёк, пополнение ресурса знаний
            foreach (var socket in Sockets)
            {
                if (!socket.IsOccupied) continue;
                
                var skill = socket.Skill;
                
                skill.Cooldown -= Time.deltaTime;

                if (skill.Cooldown > 0) continue;
            
                skill.Cooldown = skill.BaseCooldown;
                
                _intel.Increase(skill.IntelGain);
            }
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