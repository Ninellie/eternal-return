using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isLocked;
        
        public bool IsOccupied => isOccupied;
        public bool IsLocked => isLocked;
    }
}