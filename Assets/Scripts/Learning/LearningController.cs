using System;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningController : ITickable
    {
        public event Action OnLearningStarted;
        public event Action OnLearningFinished;

        public LearningProcess Process { get; private set; }

        private readonly NodeGenerator _nodeGenerator;

        private bool _isLearning;

        public LearningController(LearningProcessSettings settings)
        {
            _nodeGenerator = new NodeGenerator(settings);
        }

        public void Tick()
        {
            if (!_isLearning) return;

            Process.Tick(Time.deltaTime);

            if (Process.Duration > 0) return;
            
            Process = null;
            _isLearning = false;
            OnLearningFinished?.Invoke();
        }

        public void StartLearning()
        {
            Process = _nodeGenerator.Generate();
            _isLearning = true;
            OnLearningStarted?.Invoke();
        }
    }
}
