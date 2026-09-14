using System;
using EternalReturn.Skills;

namespace EternalReturn.Ideas
{
    [Serializable]
    public class Idea
    {
        public SkillConfig SkillConfig;
        public float HarvestCooldown;
        public float PostHarvestCooldown;
    }
}