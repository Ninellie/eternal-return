using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class Skill
    {
        [SerializeField] private string name;
        [SerializeField] private float baseCooldown;
        [SerializeField] private float cooldown;
        [SerializeField] private int intelGain;
        
        public string Name => name;
        public float BaseCooldown => baseCooldown;
        public float Cooldown => cooldown;
        public int IntelGain => intelGain;

        public event Action<int> OnComplete;

        public Skill(SkillConfig config)
        {
            name = config.Name;
            baseCooldown = config.BaseCooldown;
            intelGain = config.IntelGain;
        }
        
        public void Tick()
        {
            cooldown -= Time.deltaTime;

            if (cooldown > 0) return;
            
            cooldown = BaseCooldown;
            
            OnComplete?.Invoke(intelGain);
        }
    }
}