using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public class TopicSlotView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image filler;
        
        [SerializeField] private TopicSlot slot;

        public event Action<TopicSlot> OnCreateButtonClick;
        
        private void OnEnable()
        {
            if (slot == null) return;
            slot.OnOccupied += RefreshView;
        }
        
        private void OnDisable()
        {
            slot.OnOccupied -= RefreshView;
        }
        
        private void Update()
        {
            if (!slot.IsOccupied) return;
            
            var percent = 1 - slot.Topic.Cooldown / slot.Topic.BaseCooldown;
            
            filler.rectTransform.anchorMax = new Vector2(percent, 1);
        }
        
        /// <summary>
        /// Метод вызывается кнопкой из инспектора
        /// </summary>
        public void CreateButtonClick()
        {
            OnCreateButtonClick?.Invoke(slot);
        }
        
        public void SetSlot(TopicSlot value)
        {
            slot = value;
            SetEmpty();
            slot.OnOccupied += RefreshView;
        }
        
        public void RefreshView()
        {
            if (slot.IsOccupied)
            {
                label.text = $"{slot.Topic.Name}";
            }
            else
            {
                label.text = "Свободный слот";
            }
        }

        private void SetEmpty()
        {
            filler.rectTransform.anchorMax = new Vector2(0, 1);
        }
    }
}