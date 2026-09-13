using System;
using System.Collections.Generic;
using EternalReturn.Resources_Feature;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicsController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private List<TopicSocket> sockets;
        [SerializeField] private TopicsRepository topicRepository;
        [SerializeField] private ResourceConfigs resourceConfigs;
        
        [Header("Settings")]
        [SerializeField] private int socketIntelPrice;
        
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

        public List<TopicSocket> Sockets => sockets;

        private void FixedUpdate()
        {
            // Обработка кулдаунов тем и если кулдаун истёк, маркировка их для сбора. 
            foreach (var socket in sockets)
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
            // todo если сокет первый, то он стоит 0
            var intel = resourceConfigs.Intel;
            
            if (socketIntelPrice > intel.Amount) return;
            
            intel.Decrease(socketIntelPrice);
            
            var socket = new TopicSocket();
            Sockets.Add(socket);
            OnSocketCreated?.Invoke(socket);
        }

        public void CreateTopic(TopicSocket socket)
        {
            if (socket.IsOccupied) return;
            
            var config = topicRepository.GetRandomTopic();
            
            var topic = new DailyTopic(config);
            topic.Cooldown = topic.BaseCooldown;
            
            socket.Topic = topic;
            socket.IsOccupied = true;
            
            OnTopicInserted?.Invoke(socket);
        }

        public void HarvestTopicFromSocket(TopicSocket socket)
        {
            var motivationGain = socket.Topic.MotivationGain;
            
            resourceConfigs.Motivation.Increase(motivationGain);

            socket.Topic = null;
            socket.IsOccupied = false;
            
            OnTopicRemoved?.Invoke(socket);
        }
    }
}