using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public class TopicView : MonoBehaviour
    {
        [SerializeField] private int slotSlotId;
        [SerializeField] private RectTransform topicSlot;
        
        [SerializeField] private RectTransform tabButtonRect;
        [SerializeField] private Button tabButton;
        [SerializeField] private TMP_Text tabButtonLabel;
        
        [SerializeField] private Image alertPanel;
        
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private MessageView messageViewPrefab;
        
        private Dictionary<TopicMessage, MessageView> _messageViewsByMessage;
        
        public int SlotId => slotSlotId;

        public void Init(Topic topic, int tabsCount)
        {
            _messageViewsByMessage = new Dictionary<TopicMessage, MessageView>();
            slotSlotId = topic.SlotId;
            tabButtonLabel.text = slotSlotId.ToString();

            if (tabsCount == 0) return; 
            
            var anchorHorizontalStep = 1 / (float) tabsCount;
            
            var anchorMin = (slotSlotId - 1) * anchorHorizontalStep;
            var anchorMax = slotSlotId * anchorHorizontalStep;
            
            tabButtonRect.anchorMin = new Vector2(anchorMin, tabButtonRect.anchorMin.y);
            tabButtonRect.anchorMax = new Vector2(anchorMax, tabButtonRect.anchorMax.y);
        }
        
        public void Select()
        {
            tabButton.interactable = false;
            topicSlot.SetAsFirstSibling();
        }

        public void Deselect()
        {
            tabButton.interactable = true;
        }
        
        public void CreateMessage(TopicMessage message)
        {
            var messageView = Instantiate(messageViewPrefab, contentRoot);
            
            messageView.Init(message);
            
            _messageViewsByMessage.Add(message, messageView);
            
            messageView.transform.SetAsLastSibling();
        }

        public void DeleteMessage(TopicMessage message)
        {
            _messageViewsByMessage.Remove(message, out var messageView);
            
            messageView.gameObject.SetActive(false);
            Destroy(messageView.gameObject);
        }

        public void Tick()
        {
            foreach (var messageView in _messageViewsByMessage.Values)
            {
                messageView.UpdateDuration();
            }
        }
    }
}