using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    [Serializable]
    public class TopicSocket
    {
        [SerializeField] public bool IsOccupied;
        [SerializeField] public DailyTopic Topic;
        [SerializeField] public TopicSocketView View;
    }
}