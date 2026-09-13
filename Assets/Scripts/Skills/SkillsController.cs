using System;
using System.Collections.Generic;
using System.Linq;
using EternalReturn.Resources_Feature;
using UnityEngine;

namespace EternalReturn.Skills
{
    public class SkillsController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private List<SkillSocket> sockets;
        [SerializeField] private ResourceConfigs resourceConfigs;
        
        [Header("Settings")]
        [SerializeField] private int socketIntelPrice;
        
        /// <summary>
        /// Вызывается после того как умение заняло сокет.
        /// </summary>
        public event Action<SkillSocket> OnSocketOccupied;
        
        /// <summary>
        /// Вызывается после того как был открыт новый сокет.
        /// </summary>
        public event Action<SkillSocket> OnSocketCreated;
        
        public List<SkillSocket> Sockets => sockets;

        private void FixedUpdate()
        {
            // Обработка кулдаунов тем и если кулдаун истёк, пополнение ресурса знаний
            foreach (var socket in sockets)
            {
                if (!socket.IsOccupied) continue;
                
                var skill = socket.Skill;
                
                skill.Cooldown -= Time.deltaTime;

                if (skill.Cooldown > 0) continue;
            
                skill.Cooldown = skill.BaseCooldown;
                
                resourceConfigs.Intel.Increase(skill.IntelGain);
            }
        }

        public void CreateSocket()
        {
            // todo если сокет первый, то он стоит 0

            var intel = resourceConfigs.Intel;
            
            if (socketIntelPrice > intel.Amount) return;
            
            intel.Decrease(socketIntelPrice);
            
            var slot = new SkillSocket();
            sockets.Add(slot);
            OnSocketCreated?.Invoke(slot);
        }

        public void InsertSkill(Skill value)
        {
            var hasEmptySockets = Sockets.
                Any(s => !s.IsOccupied);
            
            if (!hasEmptySockets) return;
            
            var emptySocket = Sockets.
                First(s => !s.IsOccupied);

            emptySocket.Skill = value;
            emptySocket.IsOccupied = true;
            
            OnSocketOccupied?.Invoke(emptySocket);
        }
    }
}