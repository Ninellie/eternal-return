using System;
using EternalReturn.Skills;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class LearningController : ITickable
    {
        public event Action OnSkillChoiceOpened;
        public event Action OnSkillChoiceClosed;
        public event Action OnLearningStarted;
        public event Action OnLearningFinished;

        public bool IsChoosingSkill { get; private set; }
        public bool IsLearning { get; private set; }
        public Skill[] Offers { get; private set; }
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

        public void OpenSkillChoice()
        {
            if (IsLearning || IsChoosingSkill) return;
            if (!_skillsController.HasEmptySocket) return;

            Offers = SkillOfferRoller.Roll();
            IsChoosingSkill = true;
            OnSkillChoiceOpened?.Invoke();
        }

        public void ChooseSkill(Skill skill)
        {
            if (!IsChoosingSkill || Offers == null) return;
            if (!ContainsOffer(skill)) return;

            Offers = null;
            IsChoosingSkill = false;
            OnSkillChoiceClosed?.Invoke();
            StartLearning(skill);
        }

        private void StartLearning(Skill skill)
        {
            _reward = skill;
            Process = _nodeGenerator.Generate();
            IsLearning = true;
            OnLearningStarted?.Invoke();
        }

        private bool ContainsOffer(Skill skill)
        {
            foreach (var offer in Offers)
            {
                if (ReferenceEquals(offer, skill)) return true;
            }

            return false;
        }
    }
}
