using System.Collections.Generic;
using System.Linq;
using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class SkillPanelView : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SkillPanelController controller;
        [SerializeField] private List<SkillSlotView> viewSlots;
        
        [SerializeField] private SkillSlotView viewSlotPrefab;
        [SerializeField] private RectTransform contentContainer;
        [SerializeField] private RectTransform createButton;
        
        private void OnEnable()
        {
            foreach (var view in viewSlots)
            {
                Destroy(view.gameObject);
            }
            
            var slots = controller.Slots;

            foreach (var slot in slots)
            {
                if (slot.IsLocked) continue;

                CreateViewSlot(slot);
            }
            
            controller.OnSlotUnlocked += CreateViewSlot;
        }

        private void OnDisable()
        {
            controller.OnSlotUnlocked -= CreateViewSlot;
        }

        private void CreateViewSlot(SkillSlot slot)
        {
            var viewSlot = Instantiate(viewSlotPrefab, contentContainer);
            createButton.SetAsLastSibling();
            viewSlot.SetSlot(slot);
            viewSlot.RefreshView();
        }
    }
}