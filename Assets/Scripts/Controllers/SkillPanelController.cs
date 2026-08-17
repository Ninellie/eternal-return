using System;
using System.Collections.Generic;
using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class SkillPanelController : MonoBehaviour
    {
        [SerializeField] private List<SkillSlot> slots;

        public event Action OnSlotOccupied;
        public event Action OnSlotUnlocked;
        
        public List<SkillSlot> Slots => slots;
        
        private void AddSkill()
        {
            OnSlotOccupied?.Invoke();
        }
        
        private void UnlockSkillSlot()
        {
            OnSlotUnlocked?.Invoke();
        }
    }
}