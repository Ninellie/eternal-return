using System.Collections.Generic;
using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class SkillPanelView : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SkillPanelController controller;
        [SerializeField] private List<SkillSlotView> viewSlots;
        
        private void OnEnable()
        {
            for (var i = 0; i < viewSlots.Count; i++)
            {
                var viewSlot = viewSlots[i];
                var slot = controller.Slots[i];
                
                viewSlot.SetSlot(slot);
                viewSlot.RefreshView();
            }
        }
    }
}