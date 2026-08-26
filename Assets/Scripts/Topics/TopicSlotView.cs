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
        [SerializeField] private Button button;
        
        [SerializeField] private TopicSlot slot;
        
        [SerializeField] private DailyTopic topic;

        public event Action<TopicSlot> OnCreateClick;
        public event Action<TopicSlot> OnHarvestClick;
        
        private void Update()
        {
            if (!slot.IsOccupied) return;
            if (slot.Topic.IsHarvestable) return;
            
            var percent = 1 - slot.Topic.Cooldown / slot.Topic.BaseCooldown;
            
            filler.rectTransform.anchorMax = new Vector2(percent, 1);
        }
        
        /// <summary>
        /// Метод вызывается кнопкой из инспектора
        /// </summary>
        public void ButtonClick()
        {
            if (slot.IsOccupied)
            {
                if (topic.IsHarvestable)
                { 
                    OnHarvestClick?.Invoke(slot);
                }
            }
            else
            {
                OnCreateClick?.Invoke(slot);
            }
        }
        
        public void SetSlot(TopicSlot value)
        {
            slot = value;
            slot.OnOccupied += SetOccupied;
            slot.OnEmpty += SetEmpty;
        }

        private void SetOccupied()
        {
            topic = slot.Topic;
            topic.OnCooldownExpire += SetHarvestable;
            
            button.interactable = false;
            label.text = $"{slot.Topic.Name}";
        }

        private void SetEmpty()
        {
            if (topic != null)
            {
                topic.OnCooldownExpire -= SetHarvestable;
                topic = null;
            }
            
            filler.rectTransform.anchorMax = new Vector2(0, 1);
            button.interactable = true;
            label.text = "Свободный слот";
        }

        private void SetHarvestable()
        {
            button.interactable = true;
            label.text = $"Собрать {slot.Topic.MotivationGain} мотивации";
        }
    }
}