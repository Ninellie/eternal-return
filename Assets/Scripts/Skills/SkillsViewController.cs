using System.Collections.Generic;
using System.Linq;
using EternalReturn.Resources_Feature;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Skills
{
    public class SkillsViewController : IStartable, ITickable
    {
        private const int SocketIntelPrice = 10;
        
        private readonly SkillsController _controller;

        private readonly SkillSocketView _viewSocketPrefab;
        private readonly RectTransform _contentContainer;
        private readonly Button _buySocketButton;
        private readonly TMP_Text _buySocketButtonLabel;
        private readonly Resource _intel;

        private readonly List<SkillSocketView> _socketViews = new();
        
        public SkillsViewController(
            SkillsController controller,
            SkillSocketView viewSocketPrefab,
            RectTransform contentContainer,
            [Key("skills")] Button buySocketButton,
            [Key("skills")] TMP_Text buySocketButtonLabel,
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
            foreach (var socketView in _socketViews)
            {
                Object.Destroy(socketView.gameObject);
            }
            
            foreach (var slot in _controller.Sockets)
            {
                CreateSocketView(slot);
            }
            
            _buySocketButton.onClick.AddListener(_controller.CreateSocket);
            _controller.OnSocketCreated += CreateSocketView;
            _controller.OnSocketOccupied += SetSocketViewOccupied;
            
            _intel.OnChange += RefreshView;
            
            RefreshView(_intel.Amount);
        }

        public void Tick()
        {
            foreach (var socketView in _socketViews)
            {
                var socket = socketView.Socket;
                
                if (!socket.IsOccupied) continue;

                var skill = socket.Skill;
                
                var percent = 1 - skill.Cooldown / skill.BaseCooldown;
            
                socketView.Filler.rectTransform.anchorMax = new Vector2(percent, 1);
            }
        }

        private void CreateSocketView(SkillSocket socket)
        {
            var socketView = Object.Instantiate(_viewSocketPrefab, _contentContainer);
            
            _socketViews.Add(socketView);
            
            _buySocketButton.transform.SetAsLastSibling();

            socketView.Socket = socket;
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Label.text = "Свободный слот";
        }

        private void SetSocketViewOccupied(SkillSocket socket)
        {
            var socketView = _socketViews.First(v => v.Socket == socket);

            socketView.Label.text = $"{socketView.Socket.Skill.Name}";
        }

        private void RefreshView(int intelValue)
        {
            var isSocketsListEmpty = _controller.Sockets.Count > 0;

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

        // На будущее
        // private void SetSocketEmpty(SkillSocket socket)
        // {
        //     var socketView = socketViews.First(v => v.Socket == socket);
        //     
        //     socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
        //     socketView.Label.text = "Свободный слот";
        // }
    }
}