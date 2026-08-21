using System;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private string skill;

        public bool IsOccupied => isOccupied;
        public string Skill => skill;

        public event Action OnOccupied;
        
        public void SetSkill(string value)
        {
            skill = value;
            isOccupied = true;
            OnOccupied?.Invoke();
        }
    }
}