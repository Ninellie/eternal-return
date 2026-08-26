using System.Collections.Generic;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicsPanelView : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private TopicsPanelController controller;
        [SerializeField] private List<TopicSlotView> viewSlots;
        
        [SerializeField] private TopicSlotView viewSlotPrefab;
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

        private void CreateViewSlot(TopicSlot slot)
        {
            var viewSlot = Instantiate(viewSlotPrefab, contentContainer);
            createButton.SetAsLastSibling();
            viewSlot.SetSlot(slot);
            viewSlot.OnCreateClick += controller.AddTopicToSlot;
            viewSlot.OnHarvestClick += controller.Harvest;
        }
    }
}