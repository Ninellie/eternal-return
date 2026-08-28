using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EternalReturn.Skills
{
    public class SkillsViewController : MonoBehaviour
    {
        [Header("Dependencies")] 
        [SerializeField] private SkillsController controller;
        [SerializeField] private List<SkillSocketView> socketViews;
        
        [SerializeField] private SkillSocketView viewSocketPrefab;
        [SerializeField] private RectTransform contentContainer;
        [SerializeField] private RectTransform createButton;
        
        private void OnEnable()
        {
            foreach (var socketView in socketViews)
            {
                Destroy(socketView.gameObject);
            }
            
            foreach (var slot in controller.Sockets)
            {
                CreateSocketView(slot);
            }
            
            controller.OnSocketCreated += CreateSocketView;
            
            controller.OnSocketOccupied += SetSocketViewOccupied;
        }

        private void OnDisable()
        {
            controller.OnSocketCreated -= CreateSocketView;
            
            controller.OnSocketOccupied -= SetSocketViewOccupied;
        }

        private void Update()
        {
            foreach (var socketView in socketViews)
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
            var socketView = Instantiate(viewSocketPrefab, contentContainer);
            socketViews.Add(socketView);
            
            createButton.SetAsLastSibling();

            socketView.Socket = socket;
            
            socketView.Filler.rectTransform.anchorMax = new Vector2(0, 1);
            socketView.Label.text = "Свободный слот";
        }

        private void SetSocketViewOccupied(SkillSocket socket)
        {
            var socketView = socketViews.First(v => v.Socket == socket);

            socketView.Label.text = $"{socketView.Socket.Skill.Name}";
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