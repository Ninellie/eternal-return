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
        
        public Queue<TopicMessage> FeatureMessages { get; }
        public Queue<TopicMessage> CurrentMessages { get; }
        public float Duration { get; set; }
        public float BaseDuration { get; }
        
        private float _timeToNextMessage;

        public Topic(float baseDuration, Queue<TopicMessage> featureMessages)
        {
            BaseDuration = baseDuration;
            Duration = baseDuration;
            FeatureMessages = featureMessages;
            CurrentMessages = new Queue<TopicMessage>();
            _timeToNextMessage = featureMessages.Peek().Interval;
        }

        public bool IsDead()
        {
            return CurrentMessages.Count == 0 && Duration <= 0;
        }
        
        public void Tick(float deltaTime)
        {
            // Уменьшить длительность темы
            if (Duration > 0) Duration -= deltaTime;
            
            // Текущие сообщения ждущие набора символов
            foreach (var currentMessage in CurrentMessages.ToList())
            {
                currentMessage.Duration -= deltaTime;
                if (currentMessage.Duration > 0) continue;
                CurrentMessages.Dequeue();
                OnMessageLost?.Invoke(currentMessage);
            }
            
            // В очереди есть сообщения 
            var hasMessagesInQueue = FeatureMessages.Count > 0;
            
            // Выйти если нет
            if (!hasMessagesInQueue) return;
            
            // Уменьшить время до следующего сообщения
            _timeToNextMessage -= deltaTime;
            
            // Если время до нового сообщения вышло
            if (_timeToNextMessage > 0) return;
            
            var newMessage = FeatureMessages.Dequeue();
                
            CurrentMessages.Enqueue(newMessage);
                
            if (FeatureMessages.TryPeek(out var nextMessage))
            {
                _timeToNextMessage = nextMessage.Interval;
            }
                
            OnNewMessage?.Invoke(newMessage);
        }

        public void TryPressKey(KeyCode key)
        {
            if (!CurrentMessages.TryPeek(out var message)) return;

            message.TryPressKey(key);
            
            if (!message.IsCompleted()) return;
            
            CurrentMessages.Dequeue();
            
            OnMessageComplete?.Invoke(message);
        }
    }
}