using UnityEngine;

namespace EternalReturn.Learning
{
    public class LearningNode
    {
        public float Interval { get; }
        public float Duration { get; set; }
        public float BaseDuration { get; }
        public Vector2 Position { get; }

        public LearningNode(Vector2 position, float interval, float duration)
        {
            Position = position;
            Interval = interval;
            BaseDuration = duration;
            Duration = duration;
        }
    }
}
