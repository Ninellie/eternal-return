using System;
using System.Collections.Generic;
using EternalReturn.Stats;

namespace EternalReturn.Learning
{
    [Serializable]
    public class SkillBonus
    {
        public string StatName; 
        
        public List<FlatStatMod> FlatMods;
        public List<IncreaseStatMod> IncreaseMods;
        public List<MaxRestrictionStatMod> MaxRestrictionStatMods;
        public List<MinRestrictionStatMod> MinRestrictionStatMods;
    }
}