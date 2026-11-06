using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class Topic
    {
        public event Action<TopicMessage> OnNewMessage;
        public event Action<TopicMessage> OnMessageLost;
        public event Action<TopicMessage> OnMessageComplete;
        
        public int SlotId { get; }
        public float Duration { get; set; }
        public float BaseDuration { get; }
        
        private readonly Queue<TopicMessage> _featureMessages;
        private readonly Queue<TopicMessage> _currentMessages;
        private readonly ForumController _controller;
        private float _timeToNextMessage;

        public Topic(int slotId, float baseDuration, Queue<TopicMessage> featureMessages, ForumController controller)
        {
            SlotId = slotId;
            BaseDuration = baseDuration;
            Duration = baseDuration;
            _featureMessages = featureMessages;
            _controller = controller;
            _currentMessages = new Queue<TopicMessage>();
            _timeToNextMessage = featureMessages.Peek().Interval;
        }

        public bool IsDead()
        {
            return _currentMessages.Count == 0 && Duration <= 0;
        }
        
        public void Tick(float deltaTime)
        {
            // Уменьшить длительность темы
            if (Duration > 0) Duration -= deltaTime;
            
            // Текущие сообщения ждущие набора символов

            if (_currentMessages.TryPeek(out var message))
            {
                message.Duration -= deltaTime;
            
                if (message.Duration <= 0)
                {
                    _currentMessages.Dequeue();
                    OnMessageLost?.Invoke(message);
                    _controller.MessageLost();
                }   
            }

            // В очереди есть сообщения 
            var hasMessagesInQueue = _featureMessages.Count > 0;
            
            // Выйти если нет
            if (!hasMessagesInQueue) return;
            
            // Уменьшить время до следующего сообщения
            _timeToNextMessage -= deltaTime;
            
            // Если время до нового сообщения вышло
            if (_timeToNextMessage > 0) return;
            
            var newMessage = _featureMessages.Dequeue();
                
            _currentMessages.Enqueue(newMessage);
                
            if (_featureMessages.TryPeek(out var nextMessage))
            {
                _timeToNextMessage = nextMessage.Interval;
            }
                
            OnNewMessage?.Invoke(newMessage);
        }

        public void TryPressKey(KeyCode key)
        {
            if (!_currentMessages.TryPeek(out var message)) return;

            message.TryPressKey(key);
            
            if (!message.IsCompleted()) return;
            
            _currentMessages.Dequeue();
            
            OnMessageComplete?.Invoke(message);
        }
    }
}