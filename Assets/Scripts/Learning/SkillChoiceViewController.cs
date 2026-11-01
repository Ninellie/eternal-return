using System.Collections.Generic;
using EternalReturn.Skills;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class SkillChoiceViewController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly SkillCardView _skillViewPrefab;
        private readonly SkillsConfig _skillConfig;
        private readonly Transform _viewRoot;
        private readonly List<SkillCardView> _cards;
        private readonly Button _startLearningButton;

        public SkillChoiceViewController(
            LearningController learningController,
            SkillCardView skillViewPrefab,
            Transform viewRoot,
            SkillsConfig skillConfig,
            [Key("start-learning")] Button startLearningButton)
        {
            _learningController = learningController;
            _skillViewPrefab = skillViewPrefab;
            _viewRoot = viewRoot;
            _cards = new List<SkillCardView>();
            _skillConfig = skillConfig;
            _startLearningButton = startLearningButton;
        }

        public void Start()
        {
            _startLearningButton.onClick.AddListener(Show);
            _viewRoot.gameObject.SetActive(false);
        }

        private void Show()
        {
            var offers = _skillConfig.Roll();
            
            foreach (var skill in offers)
            {
                var card = Object.Instantiate(_skillViewPrefab, _viewRoot);
                card.Init(this, skill);
                _cards.Add(card);
            }
            
            _viewRoot.gameObject.SetActive(true);
        }

        public void OnCardSelected(Skill skill)
        {
            _learningController.StartLearning(skill);
            Hide();
        }

        private void Hide()
        {
            _viewRoot.gameObject.SetActive(false);
            
            foreach (var card in _cards)
            {
                Object.Destroy(card.gameObject);
            }

            _cards.Clear();
        }
    }
}
