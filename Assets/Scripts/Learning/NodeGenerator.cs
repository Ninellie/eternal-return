using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Learning
{
    public class NodeGenerator
    {
        private readonly LearningProcessSettings _settings;

        public NodeGenerator(LearningProcessSettings settings)
        {
            _settings = settings;
        }

        public LearningProcess Generate(LearningController controller)
        {
            var preset = RollPreset();
            var nodes = GenerateNodes(preset.ProcessDuration, preset.NodesAmount, preset.NodeDuration);
            return new LearningProcess(preset.ProcessDuration, preset.NodeRadius, nodes, controller);
        }

        private LearningPreset RollPreset()
        {
            var presets = _settings.Presets;
            if (presets == null || presets.Length == 0)
            {
                throw new InvalidOperationException("LearningProcessSettings has no presets.");
            }

            return presets[Random.Range(0, presets.Length)];
        }

        private static Queue<LearningNode> GenerateNodes(float duration, int nodesAmount, float nodeDuration)
        {
            var nodes = new Queue<LearningNode>();
            if (nodesAmount <= 0) return nodes;

            var interval = duration / nodesAmount;

            for (var i = 0; i < nodesAmount; i++)
            {
                var position = new Vector2(Random.value, Random.value);
                nodes.Enqueue(new LearningNode(position, interval, nodeDuration));
            }

            return nodes;
        }
    }
}
