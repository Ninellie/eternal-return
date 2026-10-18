using System.Collections.Generic;
using EternalReturn.Input;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningViewController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly LearningGraphic _learningGraphic;
        private readonly InputManager _inputManager;

        private bool _isLearning;

        public LearningViewController(
            LearningController learningController,
            LearningGraphic learningGraphic,
            InputManager inputManager)
        {
            _learningController = learningController;
            _learningGraphic = learningGraphic;
            _inputManager = inputManager;
        }

        public void Start()
        {
            _inputManager.OnClick += HitNodesAtPosition;
            _learningController.OnLearningStarted += OnLearningStarted;
            _learningController.OnLearningFinished += OnLearningFinished;
        }

        private void OnLearningStarted()
        {
            _learningGraphic.SetLearningProcess(_learningController.Process);
        }

        private void OnLearningFinished()
        {
            _learningGraphic.Stop();
        }

        private void HitNodesAtPosition(Vector2 position)
        {
            var process = _learningController.Process;
            if (process == null) return;

            var completedNodes = new List<LearningNode>();
            
            foreach (var node in process.CurrentNodes)
            {
                if (!_learningGraphic.IsHit(node, position)) continue;

                completedNodes.Add(node);
            }

            foreach (var completedNode in completedNodes)
            {
                process.CompleteNode(completedNode);
            }
        }
    }
}
