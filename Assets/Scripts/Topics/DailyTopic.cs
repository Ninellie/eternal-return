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
        
        public string Name => name;
        public float BaseCooldown => baseCooldown;
        public float Cooldown => cooldown;
        public int MotivationGain => motivationGain;
        
        public event Action<int> OnComplete;
        
        public DailyTopic(DailyTopicConfig config)
        {
            name = config.Name;
            baseCooldown = config.BaseCooldown;
            motivationGain = config.MotivationGain;
        }
        
        public void Tick()
        {
            cooldown -= Time.deltaTime;
        
            if (cooldown > 0) return;
            
            cooldown = BaseCooldown;
            
            OnComplete?.Invoke(motivationGain);
        }
    }
}