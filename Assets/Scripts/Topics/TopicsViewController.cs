using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicsViewController : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private TopicsController controller;
        [SerializeField] private List<TopicSocketView> socketViews;
        
        [SerializeField] private TopicSocketView viewSocketPrefab;
        [SerializeField] private RectTransform contentContainer;
        [SerializeField] private RectTransform createButton;
        
        private void OnEnable()
        {
            foreach (var socketView in socketViews)
            {
                Destroy(socketView.gameObject);
            }
            
            foreach (var socket in controller.Sockets)
            {
                CreateSocketView(socket);
            }
            
            controller.OnSocketCreated += CreateSocketView;
            
            controller.OnTopicInserted += SetSocketViewOccupied;
            controller.OnTopicCooldownExpired += SetSocketViewHarvestable;
            controller.OnTopicRemoved += SetSocketViewEmpty;
        }

        private void OnDisable()
        {
            controller.OnSocketCreated -= CreateSocketView;
            
            controller.OnTopicInserted -= SetSocketViewOccupied;
            controller.OnTopicCooldownExpired -= SetSocketViewHarvestable;
            controller.OnTopicRemoved -= SetSocketViewEmpty;
        }

        private void Update()
        {
            foreach (var socketView in socketViews)
            {
                var socket = socketView.Socket;
                
                if (!socket.IsOccupied) continue;
                
                if (socket.Topic.IsHarvestable) continue;
                
                var topic = socket.Topic;
                
                var percent = 1 - topic.Cooldown / topic.BaseCooldown;
                
                socketView.Filler.rectTransform.anchorMax = new Vector2(percent, 1);
            }
        }
        
        private void CreateSocketView(TopicSocket socket)
        {
            var socketView = Instantiate(viewSocketPrefab, contentContainer);
            socketViews.Add(socketView);
            
            createButton.SetAsLastSibling();
            
            socketView.Socket = socket;
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = "Свободный слот";
            
            var targetSocket = socket;
            socketView.Button.onClick.AddListener(() => OnSocketViewButtonClick(targetSocket));
        }

        private void OnSocketViewButtonClick(TopicSocket socket)
        {
            if (!socket.IsOccupied)
            {
                controller.CreateTopic(socket);
                return;
            }

            if (!socket.Topic.IsHarvestable) return;
            
            controller.HarvestTopicFromSocket(socket);

        }

        private void SetSocketViewOccupied(TopicSocket socket)
        {
            var socketView = socketViews.First(v => v.Socket == socket);
            
            socketView.Button.interactable = false;
            socketView.Label.text = $"{socket.Topic.Name}";
        }

        private void SetSocketViewEmpty(TopicSocket socket)
        {
            var socketView = socketViews.First(v => v.Socket == socket);
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = "Свободный слот";
        }

        private void SetSocketViewHarvestable(TopicSocket socket)
        {
            var socketView = socketViews.First(v => v.Socket == socket);
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = $"Собрать {socket.Topic.MotivationGain} мотивации";
        }
        
    }
}