using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    [Serializable]
    public class DailyTopicConfig
    {
        [SerializeField] private string name;
        [SerializeField] private float baseCooldown;
        [SerializeField] private int motivationGain;
        
        public string Name => name;
        public float BaseCooldown => baseCooldown;
        public int MotivationGain => motivationGain;
    }
}