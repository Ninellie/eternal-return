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
            
            foreach (var slot in controller.Slots)
            {
                CreateViewSlot(slot);
            }
            
            controller.OnSlotCreated += CreateViewSlot;
        }

        private void OnDisable()
        {
            controller.OnSlotCreated -= CreateViewSlot;
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