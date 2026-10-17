using System.Collections.Generic;
using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public class TopicView : MonoBehaviour
    {
        [SerializeField] private int slotId;
        [SerializeField] private RectTransform topicTab;
        
        [SerializeField] private RectTransform tabButtonRect;
        [SerializeField] private Button tabButton;
        [SerializeField] private TMP_Text tabButtonLabel;
        [SerializeField] private ButtonStyles styles;
        
        [SerializeField] private Image alertPanel;
        [SerializeField] private RectTransform durationBar;
        
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private MessageView messageViewPrefab;
        
        public int SlotId => slotId;
        
        private Dictionary<TopicMessage, MessageView> _messageViewsByMessage;
        private Topic _topic;

        public void Init(Topic topic, int tabsCount)
        {
            _topic = topic;
            _messageViewsByMessage = new Dictionary<TopicMessage, MessageView>();
            slotId = topic.SlotId;
            tabButtonLabel.text = slotId.ToString();

            if (tabsCount == 0) return; 
            
            var anchorHorizontalStep = 1 / (float) tabsCount;
            
            var anchorMin = (slotId - 1) * anchorHorizontalStep;
            var anchorMax = slotId * anchorHorizontalStep;
            
            tabButtonRect.anchorMin = new Vector2(anchorMin, tabButtonRect.anchorMin.y);
            tabButtonRect.anchorMax = new Vector2(anchorMax, tabButtonRect.anchorMax.y);
        }
        
        public void Select()
        {
            ViewHelper.SetColors(tabButton, styles.Active);
            topicTab.SetAsLastSibling();
        }

        public void Deselect()
        {
            ViewHelper.SetColors(tabButton, styles.Main);
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
            ViewHelper.UpdateDurationBar(_topic.BaseDuration, _topic.Duration, durationBar, ViewHelper.DurationBarMode.TopToBottom);
            
            foreach (var messageView in _messageViewsByMessage.Values)
            {
                messageView.UpdateDuration();
            }
        }
    }
}