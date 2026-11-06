using UnityEngine;

namespace EternalReturn.Stress
{
    [CreateAssetMenu(menuName = "Eternal Return/stress-config")]
    public class StressConfig : ScriptableObject
    {
        public int learningStressGain;
        public int forumStressGain;
    }
}