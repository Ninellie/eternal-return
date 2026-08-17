using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public void AddSkill(string skillName)
        {
            var hasEmptyUnlockedSlots = slots.
                Any(s => !s.IsOccupied && !s.IsLocked);
            
            if (!hasEmptyUnlockedSlots) return;
            
            var emptyUnlockedSlot = slots.
                First(s => !s.IsOccupied && !s.IsLocked);

            emptyUnlockedSlot.SetSkill(skillName);
            
            OnSlotOccupied?.Invoke();
        }
        
        // private void UnlockSkillSlot()
        // {
        //     OnSlotUnlocked?.Invoke();
        // }
    }
}