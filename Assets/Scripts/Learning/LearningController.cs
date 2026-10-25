using System;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningController : ITickable
    {
        public event Action OnLearningStarted;
        public event Action OnLearningFinished;

        public bool IsLearning { get; private set; }
        public LearningProcess Process { get; private set; }

        private readonly NodeGenerator _nodeGenerator;


        public LearningController(LearningProcessSettings settings)
        {
            _nodeGenerator = new NodeGenerator(settings);
        }

        public void Tick()
        {
            if (!IsLearning) return;

            Process.Tick(Time.deltaTime);

            if (Process.Duration > 0) return;
            
            Process = null;
            IsLearning = false;
            OnLearningFinished?.Invoke();
        }

        public void StartLearning()
        {
            Process = _nodeGenerator.Generate();
            IsLearning = true;
            OnLearningStarted?.Invoke();
        }
    }
}
