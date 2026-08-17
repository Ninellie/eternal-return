using System;
using TMPro;
using UnityEngine;

namespace EternalReturn.Core
{
    [Serializable]
    public class SkillSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private bool isLocked;
        [SerializeField] private string skill;
        [SerializeField] private TextMeshProUGUI label;

        public bool IsLocked => isLocked;
        public bool IsOccupied => isOccupied;

        public void SetSkill(string value)
        {
            if (isLocked) return;
            skill = value;
            isOccupied = true;
            label.text = skill;
        }
    }
}