using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public class TopicView : MonoBehaviour
    {
        [SerializeField] private int id;
        [SerializeField] private RectTransform topicSlot;
        
        [SerializeField] private RectTransform tabButtonRect;
        [SerializeField] private Button tabButton;
        [SerializeField] private TMP_Text tabButtonLabel;
        
        [SerializeField] private Image alertPanel;
        
        [SerializeField] private RectTransform contentRoot;
        [SerializeField] private TopicMessageView messageViewPrefab;
        [SerializeField] private List<TopicMessageView> messages;
        
        public int Id => id;
        public List<TopicMessageView> Messages => messages;

        // private TopicSlot _slot;
        
        public void Select()
        {
            tabButton.interactable = false;
            topicSlot.SetAsFirstSibling();
        }

        public void Deselect()
        {
            tabButton.interactable = true;
        }

        // public void SetSlot(TopicSlot slot)
        // {
        //     _slot = slot;
        // }
        
        public void SetId(int value, int tabsCount)
        {
            id = value;
            tabButtonLabel.text = id.ToString();

            if (tabsCount == 0) return; 
            
            var anchorHorizontalStep = 1 / (float) tabsCount;
            
            var anchorMin = (id - 1) * anchorHorizontalStep;
            var anchorMax = id * anchorHorizontalStep;
            
            tabButtonRect.anchorMin = new Vector2(anchorMin, tabButtonRect.anchorMin.y);
            tabButtonRect.anchorMax = new Vector2(anchorMax, tabButtonRect.anchorMax.y);
        }
        
        public void CreateMessage(TopicMessage message)
        {
            var messageView = Instantiate(messageViewPrefab, contentRoot);
            messageView.Init(message);
            Messages.Add(messageView);
            messageView.transform.SetAsLastSibling();
        }

        public void DeleteMessage(TopicMessage message)
        {
            var messageView = Messages.First(x => x.Message == message);
            Messages.Remove(messageView);
            messageView.gameObject.SetActive(false);
            Destroy(messageView.gameObject);
        }
    }
}