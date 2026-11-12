using System;
using System.Linq;
using EternalReturn.GameTime;
using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class ForumController : IStartable, ITickable
    {
        public event Action<TopicSlot> OnTopicOpened;
        public event Action<Topic> OnTopicSelected;
        public event Action<Topic> OnTopicClosed;
        public event Action<Topic> OnTopicAborted;
        public event Action OnMessageLost;
        
        public bool HasUnoccupiedSlot => _slots.Any(x => x.Status == SlotStatus.Unoccupied);
        public int SlotCount => _slots.Length;

        private readonly InputManager _inputManager;
        private readonly MessageGenerator _messageGenerator;
        private readonly TopicSlot[] _slots;
        private readonly DayCycleController _dayCycleController;
        
        private Topic _selectedTopic;
        private int _pauseCount;
        
        public ForumController(
            TopicSettings settings,
            InputManager inputManager,
            DayCycleController dayCycleController)
        {
            _inputManager = inputManager;
            _messageGenerator = new MessageGenerator(settings);
            _slots = new TopicSlot[settings.SlotsAmount];
            _dayCycleController = dayCycleController;
        }
        
        public void Start()
        {
            for (var i = 0; i < _slots.Length; i++)
            {
                _slots[i] = new TopicSlot
                {
                    Id = i + 1,
                    Status = SlotStatus.Unoccupied,
                };
            }
            
            _inputManager.OnSelectSlot += SelectTopic;
            _inputManager.OnKeyPressed += PressKey;
            _dayCycleController.OnDayEnded += Abort;
        }

        public void Pause()
        {
            _pauseCount++;
        }

        public void Resume()
        {
            _pauseCount = Mathf.Max(_pauseCount - 1, 0);
        }

        public void Tick()
        {
            if (_pauseCount > 0) return;

            foreach (var slot in _slots.Where(x => x.Status == SlotStatus.Occupied))
            {
                slot.Topic.Tick(Time.deltaTime);

                // Если тема мёртвая - надо закрыть.
                if (!slot.Topic.IsDead()) continue;

                CloseTopicInSlot(slot);
            }
        }

        private void CloseTopicInSlot(TopicSlot topicSlot)
        {
            var closedTopic = topicSlot.Topic;
            
            topicSlot.Topic = null;
            topicSlot.Status = SlotStatus.Unoccupied;
            
            OnTopicClosed?.Invoke(closedTopic);
            
            var hasOccupiedSlot = _slots.Any(x => x.Status == SlotStatus.Occupied);

            if (!hasOccupiedSlot)
            {
                _selectedTopic = null;
                return;
            }
            
            var first = _slots.First(x => x.Status == SlotStatus.Occupied);
            
            SelectTopic(first.Id);
        }

        public void CreateTopic()
        {
            var topicSlot = _slots.First(x => x.Status == SlotStatus.Unoccupied);
            
            var duration = _messageGenerator.RollDuration();
            var messages = _messageGenerator.Generate(duration);

            topicSlot.Topic = new Topic(topicSlot.Id, duration, messages, this);
            topicSlot.Status = SlotStatus.Occupied;
            
            OnTopicOpened?.Invoke(topicSlot);
            
            SelectTopic(topicSlot.Id);
            
            Debug.Log("Topic Created");
        }

        private void SelectTopic(int id)
        {
            var topicSlot = _slots.First(x => x.Id == id);
            
            if (topicSlot.Status != SlotStatus.Occupied) return;
            
            _selectedTopic = topicSlot.Topic;
            
            Debug.Log($"Topic {id} selected.");
            
            OnTopicSelected?.Invoke(topicSlot.Topic);
        }

        private void PressKey(KeyCode key)
        {
            _selectedTopic?.TryPressKey(key);
        }

        public void Abort()
        {
            foreach (var slot in _slots.Where(x => x.Status == SlotStatus.Occupied).ToList())
            {
                var topic = slot.Topic;
                slot.Topic = null;
                slot.Status = SlotStatus.Unoccupied;
                OnTopicAborted?.Invoke(topic);
            }

            _selectedTopic = null;
        }

        public void MessageLost()
        {
            OnMessageLost?.Invoke();
        }
    }
}