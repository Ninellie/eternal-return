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
        [SerializeField] private float alertBlinkSpeed = 2f;
        [SerializeField, Range(0f, 1f)] private float alertMaxAlpha = 1f;
        
        [SerializeField] private RectTransform durationBar;
        
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private MessageView messageViewPrefab;
        
        public int SlotId => slotId;
        
        private Dictionary<TopicMessage, MessageView> _messageViewsByMessage;
        private Topic _topic;


        private bool _isSelected;
        
        private bool _hasNewMessages;

        public void Init(Topic topic, int tabsCount)
        {
            _topic = topic;
            _messageViewsByMessage = new Dictionary<TopicMessage, MessageView>();
            slotId = topic.SlotId;
            tabButtonLabel.text = slotId.ToString();
            SetAlertAlpha(0f);

            if (tabsCount == 0) return; 
            
            var anchorHorizontalStep = 1 / (float) tabsCount;
            
            var anchorMin = (slotId - 1) * anchorHorizontalStep;
            var anchorMax = slotId * anchorHorizontalStep;
            
            tabButtonRect.anchorMin = new Vector2(anchorMin, tabButtonRect.anchorMin.y);
            tabButtonRect.anchorMax = new Vector2(anchorMax, tabButtonRect.anchorMax.y);
        }

        public void Tick()
        {
            if (_hasNewMessages)
            {
                SetAlertAlpha(Mathf.PingPong(Time.time * alertBlinkSpeed, alertMaxAlpha));
            }
            
            ViewHelper.UpdateDurationBar(_topic.BaseDuration, _topic.Duration, durationBar, ViewHelper.DurationBarMode.TopToBottom);
            
            foreach (var messageView in _messageViewsByMessage.Values)
            {
                messageView.UpdateDuration();
            }
        }

        public void Select()
        {
            _isSelected = true;
            _hasNewMessages = false;
            
            SetAlertAlpha(0f);
            
            ViewHelper.SetColors(tabButton, styles.Active);
            topicTab.SetAsLastSibling();
        }

        public void Deselect()
        {
            _isSelected = false;
            ViewHelper.SetColors(tabButton, styles.Main);
        }

        public void CreateMessage(TopicMessage message)
        {
            var messageView = Instantiate(messageViewPrefab, contentRoot);
            
            messageView.Init(message);
            
            _messageViewsByMessage.Add(message, messageView);
            
            messageView.transform.SetAsLastSibling();

            if (!_isSelected)
            {
                _hasNewMessages = true;
            }
        }

        public void DeleteMessage(TopicMessage message)
        {
            _messageViewsByMessage.Remove(message, out var messageView);
            
            messageView.gameObject.SetActive(false);
            Destroy(messageView.gameObject);
        }

        private void SetAlertAlpha(float alpha)
        {
            var color = alertPanel.color;
            color.a = alpha;
            alertPanel.color = color;
        }
    }
}