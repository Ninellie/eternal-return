using System;
using System.Collections.Generic;
using System.Linq;
using EternalReturn.Resources_Feature;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicsPanelController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private List<TopicSlot> slots;
        [SerializeField] private TopicsRepository topicRepository;
        [SerializeField] private ResourceRepository resourceRepository;
        
        [Header("Settings")]
        [SerializeField] private string intelResourceName;
        
        public event Action<TopicSlot> OnSlotCreated;
        
        public List<TopicSlot> Slots => slots;        
        
        private void FixedUpdate()
        {
            slots.Where(slot=>slot.IsOccupied).ToList().ForEach(slot => slot.Topic.Tick());
        }
        
        public void CreateSlot()
        {
            var slot = new TopicSlot();
            slots.Add(slot);
            OnSlotCreated?.Invoke(slot);
        }

        public void AddTopicToSlot(TopicSlot slot)
        {
            if (slot.IsOccupied) return;
            var config = topicRepository.GetRandomTopic();
            var topic = new DailyTopic(config);
            slot.SetTopic(topic);
            
            topic.OnComplete += GainMotivation;
        }
        
        private void GainMotivation(int value)
        {
            resourceRepository.GetByName(intelResourceName).Increase(value);
        }
    }
}