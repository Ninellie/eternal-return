using System;
using System.Collections.Generic;

namespace EternalReturn.Stats
{
    public class Stat
    {
        public string Name;
        
        public int BaseValue { get; private set; }
        public int MaxValue { get; private set; }
        public int MinValue { get; private set; }
        
        public List<FlatStatMod> FlatMods; // Плоские добавочные значения
        public List<IncreaseStatMod> IncreaseMods; // Мультипликаторы
        public List<MaxRestrictionStatMod> MaxRestrictionStatMods; // Ограничивает значение сверху
        public List<MinRestrictionStatMod> MinRestrictionStatMods; // Ограничивает значение снизу

        public Stat(StatConfig config)
        {
            
        }
        
        public int GetValue()
        {
            throw new NotImplementedException();
        }
    }
}