using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    [Serializable]
    public class DailyTopic
    {
        [SerializeField] private string name;
        [SerializeField] private float baseCooldown;
        [SerializeField] private float cooldown;
        [SerializeField] private int motivationGain;
        [SerializeField] private bool isHarvestable;
        
        public string Name => name;
        public float BaseCooldown => baseCooldown;
        public float Cooldown => cooldown;
        public int MotivationGain => motivationGain;
        public bool IsHarvestable => isHarvestable;
        
        public event Action OnCooldownExpire;
        
        public DailyTopic(DailyTopicConfig config)
        {
            name = config.Name;
            baseCooldown = config.BaseCooldown;
            motivationGain = config.MotivationGain;
        }

        public void SetOnCooldown()
        {
            isHarvestable = false;
            cooldown = baseCooldown;
        }
        
        public void Tick()
        {
            if (isHarvestable) return;
            
            cooldown -= Time.deltaTime;
        
            if (cooldown > 0) return;
            
            cooldown = 0;
            
            isHarvestable = true;
            
            OnCooldownExpire?.Invoke();
        }
    }
}