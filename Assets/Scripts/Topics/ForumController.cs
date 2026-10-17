using System;
using System.Linq;
using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class ForumController : IStartable, ITickable
    {
        public event Action<TopicSlot> OnTopicOpened;
        public event Action<TopicSlot> OnTopicSlotSelected;
        public event Action<Topic> OnTopicClosed;
        
        public bool HasUnoccupiedSlot => _slots.Any(x => x.Status == SlotStatus.Unoccupied);
        
        private readonly InputManager _inputManager;
        private readonly MessageGenerator _messageGenerator;
        private readonly TopicSlot[] _slots;
        
        public ForumController(TopicSettings settings, InputManager inputManager)
        {
            _inputManager = inputManager;
            _messageGenerator = new MessageGenerator(settings);
            _slots = new TopicSlot[settings.SlotsAmount];
        }
        
        public void Start()
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new TopicSlot
                {
                    Id = i + 1,
                    Status = SlotStatus.Locked,
                    IsSelected = false
                };
            }
            
            _slots[0].Status = SlotStatus.Unoccupied;
            _slots[0].IsSelected = true;

            _slots[1].Status = SlotStatus.Unoccupied;
            
            _inputManager.OnSelectSlot += SelectTopic;
            _inputManager.OnKeyPressed += PressKey;
        }

        public void Tick()
        {
            foreach (var slot in _slots.Where(x => x.Status == SlotStatus.Occupied))
            {
                var topic = slot.Topic;
                
                topic.Tick(Time.deltaTime);

                // Если тема мёртвая - надо закрыть.
                if (!topic.IsDead()) continue;

                slot.Topic = null;
                slot.Status = SlotStatus.Unoccupied;
                
                OnTopicClosed?.Invoke(topic);
            }
        }

        public void CreateTopic()
        {
            var slot = _slots.First(x => x.Status == SlotStatus.Unoccupied);
            
            var duration = _messageGenerator.RollDuration();
            var messages = _messageGenerator.Generate(duration);

            slot.Topic = new Topic(slot.Id, duration, messages);
            slot.Status = SlotStatus.Occupied;

            OnTopicOpened?.Invoke(slot);
            Debug.Log("Topic Created");
        }

        private void SelectTopic(int id)
        {
            if (_slots[id - 1].Status != SlotStatus.Occupied) return;
            
            var selectableSlots = _slots.Where(x => x.Status == SlotStatus.Occupied);
            
            foreach (var slot in selectableSlots)
            {
                slot.IsSelected = slot.Id == id;
            }
            
            OnTopicSlotSelected?.Invoke(_slots[id - 1]);
        }

        private void PressKey(KeyCode key)
        {
            var slot = _slots.First(x => x.IsSelected);
            
            if (slot.Status is not SlotStatus.Occupied) return;
            
            var selectedTopic = slot.Topic;

            selectedTopic.TryPressKey(key);
        }
    }
}