using System;
using UnityEngine;

namespace EternalReturn.Learning
{
    [Serializable]
    public class SkillConfig
    {
        public string Discipline;
        public string Description;
        public string BonusDescription;
        public Sprite Icon;
        
        public SkillBonus[] Bonus;
    }
}