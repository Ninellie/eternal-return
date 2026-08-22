using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillConfig
    {
        [SerializeField] private string name;
        [SerializeField] private float baseCooldown;
        [SerializeField] private int intelGain;
        
        public string Name => name;
        public float BaseCooldown => baseCooldown;
        public int IntelGain => intelGain;
    }
}