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
        private readonly List<SkillCardView> _cards;
        private readonly Button _startLearningButton;
        private readonly SkillChoicePanel _skillChoicePanel;

        public SkillChoiceViewController(
            LearningController learningController,
            SkillCardView skillViewPrefab,
            SkillsConfig skillConfig,
            [Key("start-learning")] Button startLearningButton,
            SkillChoicePanel skillChoicePanel)
        {
            _learningController = learningController;
            _skillViewPrefab = skillViewPrefab;
            _cards = new List<SkillCardView>();
            _skillConfig = skillConfig;
            _startLearningButton = startLearningButton;
            _skillChoicePanel = skillChoicePanel;
        }

        public void Start()
        {
            _startLearningButton.onClick.AddListener(Show);
            _skillChoicePanel.gameObject.SetActive(false);
        }

        private void Show()
        {
            var offers = _skillConfig.Roll();
            
            foreach (var skill in offers)
            {
                var card = Object.Instantiate(_skillViewPrefab, _skillChoicePanel.CardRoot);
                card.Init(this, skill);
                _cards.Add(card);
            }
            
            _skillChoicePanel.gameObject.SetActive(true);
        }

        public void OnCardPointerEnter(Skill skill)
        {
            _skillChoicePanel.Discipline.text = skill.Discipline;
            _skillChoicePanel.Description.text = skill.Description;
            _skillChoicePanel.Bonus.text = skill.BonusDescription;
        }
        
        public void OnCardPointerExit()
        {
            _skillChoicePanel.Discipline.text = string.Empty;
            _skillChoicePanel.Description.text = string.Empty;
            _skillChoicePanel.Bonus.text = string.Empty;
        }

        public void OnCardSelected(Skill skill)
        {
            _learningController.StartLearning(skill);
            Hide();
        }

        private void Hide()
        {
            _skillChoicePanel.gameObject.SetActive(false);
            
            foreach (var card in _cards)
            {
                Object.Destroy(card.gameObject);
            }

            _cards.Clear();
        }
    }
}
