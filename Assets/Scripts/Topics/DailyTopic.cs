using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    [Serializable]
    public class DailyTopic
    {
        [SerializeField] public string Name;
        [SerializeField] public float BaseCooldown;
        [SerializeField] public float Cooldown;
        [SerializeField] public int MotivationGain;
        [SerializeField] public bool IsHarvestable;
        
        public DailyTopic(DailyTopicConfig config)
        {
            Name = config.Name;
            BaseCooldown = config.BaseCooldown;
            MotivationGain = config.MotivationGain;
        }
    }
}