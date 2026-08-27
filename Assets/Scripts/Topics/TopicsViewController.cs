using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicsViewController : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private TopicsController controller;
        [SerializeField] private List<TopicSocketView> viewSockets;
        
        [SerializeField] private TopicSocketView viewSocketPrefab;
        [SerializeField] private RectTransform contentContainer;
        [SerializeField] private RectTransform createButton;
        
        private void OnEnable()
        {
            foreach (var viewSocket in viewSockets)
            {
                Destroy(viewSocket.gameObject);
            }
            
            foreach (var socket in controller.Sockets)
            {
                CreateViewSocket(socket);
            }
            
            controller.OnSocketCreated += CreateViewSocket;
            
            controller.OnTopicInserted += SetSocketOccupied;
            controller.OnTopicCooldownExpired += SetSocketHarvestable;
            controller.OnTopicRemoved += SetSocketEmpty;
        }

        private void OnDisable()
        {
            controller.OnSocketCreated -= CreateViewSocket;
            
            controller.OnTopicInserted -= SetSocketOccupied;
            controller.OnTopicCooldownExpired -= SetSocketHarvestable;
            controller.OnTopicRemoved -= SetSocketEmpty;
        }

        private void Update()
        {
            foreach (var viewSocket in viewSockets)
            {
                var socket = viewSocket.Socket;
                
                if (!socket.IsOccupied) continue;
                
                if (socket.Topic.IsHarvestable) continue;
                
                var topic = socket.Topic;
                
                var percent = 1 - topic.Cooldown / topic.BaseCooldown;
                
                viewSocket.Filler.rectTransform.anchorMax = new Vector2(percent, 1);
            }
        }
        
        private void CreateViewSocket(TopicSocket socket)
        {
            var viewSocket = Instantiate(viewSocketPrefab, contentContainer);
            createButton.SetAsLastSibling();
            viewSocket.Socket = socket;
            
            var targetSocket = socket;
            viewSocket.Button.onClick.AddListener(() => OnViewSocketButtonClick(targetSocket));
            
            viewSockets.Add(viewSocket);
        }

        private void OnViewSocketButtonClick(TopicSocket socket)
        {
            if (!socket.IsOccupied)
            {
                controller.CreateTopic(socket);
                return;
            }

            if (!socket.Topic.IsHarvestable) return;
            
            controller.HarvestTopicFromSocket(socket);

        }

        private void SetSocketOccupied(TopicSocket socket)
        {
            var viewSocket = viewSockets.First(v => v.Socket == socket);
            
            viewSocket.Button.interactable = false;
            viewSocket.Label.text = $"{socket.Topic.Name}";
        }

        private void SetSocketEmpty(TopicSocket socket)
        {
            var viewSocket = viewSockets.First(v => v.Socket == socket);
            
            viewSocket.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            viewSocket.Button.interactable = true;
            viewSocket.Label.text = "Свободный слот";
        }

        private void SetSocketHarvestable(TopicSocket socket)
        {
            var viewSocket = viewSockets.First(v => v.Socket == socket);
            
            viewSocket.Button.interactable = true;
            viewSocket.Label.text = $"Собрать {socket.Topic.MotivationGain} мотивации";
        }
        
    }
}