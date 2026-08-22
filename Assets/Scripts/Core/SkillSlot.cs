using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private Skill skill;

        public bool IsOccupied => isOccupied;
        public Skill Skill => skill;
        
        public event Action OnOccupied;
        
        public void SetSkill(Skill value)
        {
            skill = value;
            isOccupied = true;
            OnOccupied?.Invoke();
        }
    }
}