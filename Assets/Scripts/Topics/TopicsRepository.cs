using System.Collections.Generic;
using UnityEngine;

namespace EternalReturn.Topics
{
    [CreateAssetMenu(menuName = "Eternal Return/Topics Repository")]
    public class TopicsRepository : ScriptableObject
    {
        [SerializeField] private List<DailyTopicConfig> dailyTopics;

        public DailyTopicConfig GetRandomTopic()
        {
            var r = Random.Range(0, dailyTopics.Count);
            return dailyTopics[r];
        }
    }
}