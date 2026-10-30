using System;
using EternalReturn.Skills;
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
        private readonly SkillsController _skillsController;
        
        private Skill _reward;

        public LearningController(LearningProcessSettings settings, SkillsController skillsController)
        {
            _nodeGenerator = new NodeGenerator(settings);
            _skillsController = skillsController;
        }

        public void Tick()
        {
            if (!IsLearning) return;

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
            Process = _nodeGenerator.Generate();
            IsLearning = true;
            OnLearningStarted?.Invoke();
        }
    }
}
