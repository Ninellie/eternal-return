using System;
using System.Collections.Generic;
using System.Linq;
using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class SkillPanelController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private List<SkillSlot> slots;
        [SerializeField] private ResourceRepository resourceRepository;
        
        [Header("Settings")]
        [SerializeField] private string intelResourceName;

        public event Action OnSlotOccupied;
        public event Action<SkillSlot> OnSlotCreated;
        
        public List<SkillSlot> Slots => slots;

        private void FixedUpdate()
        {
            slots.Where(s=>s.IsOccupied).ToList().ForEach(s => s.Skill.Tick());
        }

        public void AddSkill(Skill value)
        {
            var hasEmptyUnlockedSlots = Slots.
                Any(s => !s.IsOccupied);
            
            if (!hasEmptyUnlockedSlots) return;
            
            var emptyUnlockedSlot = Slots.
                First(s => !s.IsOccupied);

            emptyUnlockedSlot.SetSkill(value);
            
            value.OnComplete += GainIntel;
            
            OnSlotOccupied?.Invoke();
        }
        
        public void CreateSkillSlot()
        {
            var slot = new SkillSlot();
            slots.Add(slot);
            OnSlotCreated?.Invoke(slot);
        }

        private void GainIntel(int value)
        {
            resourceRepository.GetByName(intelResourceName).Increase(value);
        }
    }
}