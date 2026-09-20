using System;
using System.Collections.Generic;
using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class TopicsController : IFixedTickable
    {
        private const int SocketIntelPrice = 10;
        
        public List<TopicSocket> Sockets { get; } = new();
        
        private readonly TopicsRepository _topicRepository;

        private readonly Resource _intel;
        private readonly Resource _motivation;

        public TopicsController(TopicsRepository topicRepository, ResourceProvider resourceProvider)
        {
            _topicRepository = topicRepository;
            _intel = resourceProvider.Intel;
            _motivation = resourceProvider.Motivation;
        }

        /// <summary>
        /// Вызывается сразу после создания слота.
        /// </summary>
        public event Action<TopicSocket> OnSocketCreated;
        
        /// <summary>
        /// Вызывается сразу после заполнения слота темой.
        /// </summary>
        public event Action<TopicSocket> OnTopicInserted;  
        
        /// <summary>
        /// Вызывается сразу после того, как кулдаун темы истёк и её можно собирать.
        /// </summary>
        public event Action<TopicSocket> OnTopicCooldownExpired;
        
        /// <summary>
        /// Сразу после сбора темы и освобождения слота.
        /// </summary>
        public event Action<TopicSocket> OnTopicRemoved;


        public void FixedTick()
        {
            // Обработка кулдаунов тем и если кулдаун истёк, маркировка их для сбора. 
            foreach (var socket in Sockets)
            {
                if (!socket.IsOccupied) continue;
                
                var topic = socket.Topic;
                
                if (topic.IsHarvestable) continue;
                
                topic.Cooldown -= Time.deltaTime;
                
                if (topic.Cooldown > 0) continue;

                topic.Cooldown = 0;
                
                topic.IsHarvestable = true;
                
                OnTopicCooldownExpired?.Invoke(socket);
            }
        }

        public void CreateSocket()
        {
            var price = SocketIntelPrice;
            
            if (Sockets.Count == 0)
            {
                price = 0;
            }
            
            if (price > _intel.Amount) return;
            
            _intel.Decrease(SocketIntelPrice);
            
            var socket = new TopicSocket();
            Sockets.Add(socket);
            OnSocketCreated?.Invoke(socket);
        }

        public void CreateTopic(TopicSocket socket)
        {
            if (socket.IsOccupied) return;
            
            var config = _topicRepository.GetRandomTopic();
            
            var topic = new DailyTopic(config);
            topic.Cooldown = topic.BaseCooldown;
            
            socket.Topic = topic;
            socket.IsOccupied = true;
            
            OnTopicInserted?.Invoke(socket);
        }

        public void HarvestTopicFromSocket(TopicSocket socket)
        {
            var motivationGain = socket.Topic.MotivationGain;
            
            _motivation.Increase(motivationGain);

            socket.Topic = null;
            socket.IsOccupied = false;
            
            OnTopicRemoved?.Invoke(socket);
        }
    }
}