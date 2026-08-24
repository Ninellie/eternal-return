using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    [Serializable]
    public class TopicSlot
    {
        [SerializeField] private bool isOccupied;
        [SerializeField] private DailyTopic topic;
        
        public bool IsOccupied => isOccupied;
        public DailyTopic Topic => topic;
        
        public event Action OnOccupied;
        public event Action OnEmpty;
        
        public void SetTopic(DailyTopic value)
        {
            topic = value;
            isOccupied = true;
            OnOccupied?.Invoke();
        }

        public void Empty()
        {
            topic = null;
            isOccupied = false; 
            OnEmpty?.Invoke();
        }
    }
}