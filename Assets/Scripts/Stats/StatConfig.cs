using System;

namespace EternalReturn.Stats
{
    [Serializable]
    public class StatConfig
    {
        public string Name;
        public float BaseValue;
        public float MaxValue;
        public float MinValue;
    }
}