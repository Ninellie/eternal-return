using System;
using EternalReturn.GameTime;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningController : IStartable, ITickable
    {
        public event Action OnLearningStarted;
        public event Action OnLearningFinished;
        public event Action OnLearningAborted;
        public event Action<LearningNode> OnNewNode;
        public event Action OnNodeLost;
        public event Action OnNodeComplete;
        
        public bool IsLearning { get; private set; }
        public LearningProcess Process { get; private set; }

        private readonly NodeGenerator _nodeGenerator;
        private readonly LearningProcessSettings _config;
        private readonly SkillsController _skillsController;
        private readonly DayCycleController _dayCycleController;
        
        private Skill _reward;
        private int _pauseCount;

        public LearningController(
            LearningProcessSettings settings,
            SkillsController skillsController,
            DayCycleController dayCycleController)
        {
            _config = settings;
            _nodeGenerator = new NodeGenerator(settings);
            _skillsController = skillsController;
            _dayCycleController = dayCycleController;
        }

        public void Start()
        {
            _dayCycleController.OnDayEnded += Abort;
        }

        public void Pause()
        {
            _pauseCount++;
        }

        public void Resume()
        {
            _pauseCount = Mathf.Max(_pauseCount - 1, 0);
        }

        public void Tick()
        {
            if (!IsLearning) return;
            if (_pauseCount > 0) return;

            Process.Tick(Time.deltaTime);

            if (Process.Duration > 0) return;

            var reward = _reward;
            _reward = null;
            Process = null;
            IsLearning = false;

            if (reward != null)
            {
                _skillsController.TryPlaceInNearestSocket(reward);
            }

            OnLearningFinished?.Invoke();
        }
        
        public void StartLearning(Skill skill)
        {
            _reward = skill;
            Process = _nodeGenerator.Generate(this);
            IsLearning = true;
            OnLearningStarted?.Invoke();
        }

        public void Abort()
        {
            if (!IsLearning) return;

            _reward = null;
            Process = null;
            IsLearning = false;
            OnLearningAborted?.Invoke();
        }

        public void NodeLost()
        {
            OnNodeLost?.Invoke();
        }

        public void NodeComplete()
        {
            OnNodeComplete?.Invoke();
        }
    }
}
