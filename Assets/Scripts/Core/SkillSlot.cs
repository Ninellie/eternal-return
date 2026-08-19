using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isLocked;
        [SerializeField] private string skill;

        public bool IsLocked => isLocked;
        public bool IsOccupied => isOccupied;
        public string Skill => skill;

        public event Action OnChanged;
        
        public void SetSkill(string value)
        {
            if (isLocked) return;
            skill = value;
            isOccupied = true;
            OnChanged?.Invoke();
            
        }
    }
}