using System;
using UnityEngine;

namespace EternalReturn.Skills
{
    [Serializable]
    public class Skill
    {
        [SerializeField] public string Name;
        [SerializeField] public float BaseCooldown;
        [SerializeField] public float Cooldown;
        [SerializeField] public int IntelGain;

        public Skill(SkillConfig config)
        {
            Name = config.Name;
            BaseCooldown = config.BaseCooldown;
            IntelGain = config.IntelGain;
        }
    }
}