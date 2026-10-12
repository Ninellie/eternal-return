using System;
using System.Linq;
using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class ForumController : IStartable, ITickable
    {
        private readonly InputManager _inputManager;
        private readonly MessageGenerator _messageGenerator;
        private readonly int _slotsAmount;
        private readonly TopicSlot[] _slots;
        
        public event Action<Topic> OnTopicOpened;
        public event Action<Topic> OnTopicClosed;
        public event Action<TopicSlot> OnTopicSlotSelected;
        
        public ForumController(TopicSettings settings, InputManager inputManager)
        {
            _inputManager = inputManager;
            _messageGenerator = new MessageGenerator(settings);
            _slotsAmount = settings.SlotsAmount;
            _slots = new TopicSlot[_slotsAmount];
        }
        
        public void Start()
        {
            for (var i = 0; i < _slotsAmount; i++)
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

            _inputManager.OnSelectSlot += SelectTopic;
            _inputManager.OnKeyPressed += PressKey;
        }

        public void Tick()
        {
            foreach (var slot in _slots)
            {
                if (slot.Status is not SlotStatus.Occupied) continue;

                var topic = slot.Topic;
                topic.Tick(Time.deltaTime);

                // Если тема мёртвая - надо закрыть.
                if (!topic.IsDead()) continue;
                
                OnTopicClosed?.Invoke(topic);

                slot.Topic = null;
                slot.Status = SlotStatus.Unoccupied;    
            }
        }

        public void CreateTopic()
        {
            var slot = _slots.First(x => x.IsSelected);
            if (slot.Status is not SlotStatus.Unoccupied) return;

            var duration = _messageGenerator.RollDuration();
            var messages = _messageGenerator.Generate(duration);

            slot.Topic = new Topic(duration, messages);
            slot.Status = SlotStatus.Occupied;

            OnTopicOpened?.Invoke(slot.Topic);
            Debug.Log("Topic Created");
        }

        private void SelectTopic(int id)
        {
            foreach (var slot in _slots)
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