using System.Collections.Generic;
using System.Linq;

namespace EternalReturn.Learning
{
    public class LearningProcess
    {
        public float Duration { get; private set; }
        public float BaseDuration { get; }
        public float NodeRadius { get; }
        public List<LearningNode> CurrentNodes { get; }

        private readonly Queue<LearningNode> _featureNodes;
        private readonly LearningController _controller;
        
        private float _timeToNextNode;

        public LearningProcess(float baseDuration, float nodeRadius, Queue<LearningNode> featureNodes, LearningController controller)
        {
            BaseDuration = baseDuration;
            Duration = baseDuration;
            NodeRadius = nodeRadius;
            _featureNodes = featureNodes;
            _controller = controller;
            CurrentNodes = new List<LearningNode>();
            _timeToNextNode = featureNodes.Peek().Interval;
        }
        
        public void Tick(float deltaTime)
        {
            // Уменьшить длительность
            if (Duration > 0) Duration -= deltaTime;
            
            // Текущие узлы ждущие выполнения
            foreach (var node in CurrentNodes.ToList())
            {
                node.Duration -= deltaTime;
                if (node.Duration > 0) continue;
                CurrentNodes.Remove(node);
                _controller.NodeLost();
            }
            
            // В очереди есть узлы
            var hasNodesInQueue = _featureNodes.Count > 0;
            
            // Выйти если нет
            if (!hasNodesInQueue) return;
            
            // Уменьшить время до появления следующего узла
            _timeToNextNode -= deltaTime;
            
            // Если время до нового узла вышло
            if (_timeToNextNode > 0) return;
            
            var newMessage = _featureNodes.Dequeue();
                
            CurrentNodes.Add(newMessage);
                
            if (_featureNodes.TryPeek(out var nextMessage))
            {
                _timeToNextNode = nextMessage.Interval;
            }
                
            // OnNewNode?.Invoke(newMessage);
        }

        public void CompleteNode(LearningNode node)
        {
            if (!CurrentNodes.Remove(node)) return;
            _controller.NodeComplete();
        }
    }
}
