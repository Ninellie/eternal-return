using System.Collections.Generic;
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
        [SerializeField] private List<RectTransform> messages;
        
        public int Id => id;
        
        public void Select()
        {
            tabButton.interactable = false;
            topicSlot.SetAsFirstSibling();
        }

        public void Deselect()
        {
            tabButton.interactable = true;
        }

        public void SetId(int value, int tabsCount)
        {
            id = value;
            tabButtonLabel.text = value.ToString();

            if (tabsCount == 0) return; 
            
            var anchorHorizontalStep = 1 / (float) tabsCount;
            
            var anchorMin = (id - 1) * anchorHorizontalStep;
            var anchorMax = id * anchorHorizontalStep;
            
            tabButtonRect.anchorMin = new Vector2(anchorMin, tabButtonRect.anchorMin.y);
            tabButtonRect.anchorMax = new Vector2(anchorMax, tabButtonRect.anchorMax.y);
        }
        
        public void Lock()
        {
            
        }
    }
}