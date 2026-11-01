using System.Collections.Generic;
using EternalReturn.Input;
using EternalReturn.Topics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningViewController : IStartable, ITickable
    {
        private readonly LearningController _learningController;
        private readonly LearningGraphic _learningGraphic;
        private readonly InputManager _inputManager;
        private readonly RectTransform _durationBar;

        public LearningViewController(
            LearningController learningController,
            LearningGraphic learningGraphic,
            InputManager inputManager,
            [Key("learning-duration")] RectTransform durationBar)
        {
            _learningController = learningController;
            _learningGraphic = learningGraphic;
            _inputManager = inputManager;
            _durationBar = durationBar;
        }

        public void Start()
        {
            _inputManager.OnClick += HitNodesAtPosition;
            _learningController.OnLearningStarted += OnLearningStarted;
            _learningController.OnLearningFinished += OnLearningFinished;
            _learningController.OnLearningAborted += OnLearningAborted;
            _durationBar.gameObject.SetActive(false);
        }

        public void Tick()
        {
            if (!_learningController.IsLearning) return;

            UpdateDurationBar();
        }

        private void OnLearningStarted()
        {
            _learningGraphic.SetLearningProcess(_learningController.Process);
            _durationBar.gameObject.SetActive(true);
            UpdateDurationBar();
        }

        private void OnLearningFinished()
        {
            _learningGraphic.Stop();
            _durationBar.gameObject.SetActive(false);
        }

        private void OnLearningAborted()
        {
            _learningGraphic.Stop();
            _durationBar.gameObject.SetActive(false);
        }

        private void UpdateDurationBar()
        {
            var process = _learningController.Process;
            ViewHelper.UpdateDurationBar(
                process.BaseDuration,
                process.Duration,
                _durationBar,
                ViewHelper.DurationBarMode.RightToLeft);
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
