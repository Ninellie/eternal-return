using System;

namespace EternalReturn.Stats
{
    [Serializable]
    public class StatConfig
    {
        public string Name;
        public int BaseValue;
        public int MaxValue;
        public int MinValue;
    }
}