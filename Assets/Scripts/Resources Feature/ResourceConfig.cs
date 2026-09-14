using System;

namespace EternalReturn.Resources_Feature
{
    [Serializable]
    public struct ResourceConfig
    {
        public ResourceName Name;
        
        public int Amount;
        public int MaxAmount;
        public bool EmptyOnFill;
        public bool IsIncreaseBlocked;
    }
}