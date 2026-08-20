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
        public event Action<SkillSlot> OnSlotUnlocked;
        
        public List<SkillSlot> Slots => slots;
        
        public void AddSkill(string skillName)
        {
            var hasEmptyUnlockedSlots = Slots.
                Any(s => !s.IsOccupied && !s.IsLocked);
            
            if (!hasEmptyUnlockedSlots) return;
            
            var emptyUnlockedSlot = Slots.
                First(s => !s.IsOccupied && !s.IsLocked);

            emptyUnlockedSlot.SetSkill(skillName);
            
            OnSlotOccupied?.Invoke();
        }
        
        public void CreateSkillSlot()
        {
            var slot = new SkillSlot();
            slot.Unlock();
            OnSlotUnlocked?.Invoke(slot);
        }
    }
}