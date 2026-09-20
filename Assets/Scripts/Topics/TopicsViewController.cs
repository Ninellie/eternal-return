using System.Collections.Generic;
using System.Linq;
using EternalReturn.Resources_Feature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class TopicsViewController : IStartable, ITickable
    {
        private const int SocketIntelPrice = 10;
        
        private readonly TopicsController _controller;
        
        private readonly TopicSocketView _viewSocketPrefab;
        private readonly RectTransform _contentContainer;
        private readonly Button _buySocketButton;
        private readonly TextMeshProUGUI _buySocketButtonLabel;

        private readonly List<TopicSocketView> _socketViews = new();
        private readonly Resource _intel;

        public TopicsViewController(
            TopicsController controller,
            TopicSocketView viewSocketPrefab,
            [Key("topics")] RectTransform contentContainer,
            [Key("topics")] Button buySocketButton,
            [Key("topics")] TextMeshProUGUI buySocketButtonLabel,
            ResourceProvider resourceProvider)
        {
            _controller = controller;
            _viewSocketPrefab = viewSocketPrefab;
            _contentContainer = contentContainer;
            _buySocketButton = buySocketButton;
            _buySocketButtonLabel = buySocketButtonLabel;
            _intel = resourceProvider.Intel;
        }

        public void Start()
        {
            foreach (var socket in _controller.Sockets)
            {
                CreateSocketView(socket);
            }
            
            _controller.OnSocketCreated += CreateSocketView;
            
            _controller.OnTopicInserted += SetSocketViewOccupied;
            _controller.OnTopicCooldownExpired += SetSocketViewHarvestable;
            _controller.OnTopicRemoved += SetSocketViewEmpty;
            
            _intel.OnChange += RefreshView;
            
            RefreshView(_intel.Amount);
            
            _buySocketButton.onClick.AddListener(_controller.CreateSocket);
        }
        
        public void Tick()
        {
            foreach (var socketView in _socketViews)
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
            var socketView = Object.Instantiate(_viewSocketPrefab, _contentContainer);
            
            _socketViews.Add(socketView);
            
            _buySocketButton.transform.SetAsLastSibling();
            
            socketView.Socket = socket;
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = "Свободный слот";
            
            var targetSocket = socket;
            socketView.Button.onClick.AddListener(() => OnSocketViewButtonClick(targetSocket));
            
            RefreshView(_intel.Amount);
        }

        private void OnSocketViewButtonClick(TopicSocket socket)
        {
            if (!socket.IsOccupied)
            {
                _controller.CreateTopic(socket);
                return;
            }

            if (!socket.Topic.IsHarvestable) return;
            
            _controller.HarvestTopicFromSocket(socket);
        }

        private void SetSocketViewOccupied(TopicSocket socket)
        {
            var socketView = _socketViews.First(v => v.Socket == socket);
            
            socketView.Button.interactable = false;
            socketView.Label.text = $"{socket.Topic.Name}";
        }

        private void SetSocketViewEmpty(TopicSocket socket)
        {
            var socketView = _socketViews.First(v => v.Socket == socket);
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = "Свободный слот";
        }

        private void SetSocketViewHarvestable(TopicSocket socket)
        {
            var socketView = _socketViews.First(v => v.Socket == socket);
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Button.interactable = true;
            socketView.Label.text = $"Собрать {socket.Topic.MotivationGain} мотивации";
        }
        
        private void RefreshView(int intelValue)
        {
            var isSocketsListEmpty = _controller.Sockets.Count == 0;

            var price = SocketIntelPrice;
            
            if (isSocketsListEmpty)
            {
                price = 0;
                _buySocketButtonLabel.text = "Открыть сокет";   
            }
            else
            {
                _buySocketButtonLabel.text = $"Купить сокет за {SocketIntelPrice} знаний";   
            }
            
            var isCanBuySocket = intelValue >= price;

            _buySocketButton.interactable = isCanBuySocket;
        }
    }
}