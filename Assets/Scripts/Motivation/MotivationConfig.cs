using UnityEngine;

namespace EternalReturn.Motivation
{
    [CreateAssetMenu(menuName = "Eternal Return/motivation-config")]
    public class MotivationConfig : ScriptableObject
    {
        [SerializeField] private int startLearningMotivationCost = 10;
        [SerializeField] private int nodeCompleteMotivationCost = 1;
        [SerializeField] private int nodeCompleteStressGain = 1;
        [SerializeField] private int forumMotivationGain = 1;

        public int StartLearningMotivationCost => startLearningMotivationCost;
        public int NodeCompleteMotivationCost => nodeCompleteMotivationCost;
        public int NodeCompleteStressGain => nodeCompleteStressGain;
        public int ForumMotivationGain => forumMotivationGain;
    }
}