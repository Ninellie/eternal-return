using EternalReturn.Motivation;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using EternalReturn.Topics;
using TMPro;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class StartLearningButtonViewController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly SkillsController _skillsController;
        private readonly Button _startLearningButton;
        private readonly Resource _motivation;
        private readonly MotivationConfig _config;

        public StartLearningButtonViewController(
            LearningController learningController,
            SkillsController skillsController,
            [Key("start-learning")] Button startLearningButton,
            ResourceProvider resourceProvider, 
            MotivationConfig config)
        {
            _learningController = learningController;
            _skillsController = skillsController;
            _startLearningButton = startLearningButton;
            _config = config;
            _motivation = resourceProvider.Motivation;
        }

        public void Start()
        {
            _skillsController.OnSocketsChanged += UpdateButtonState;
            _learningController.OnLearningStarted += UpdateButtonState;
            _learningController.OnLearningFinished += UpdateButtonState;
            _learningController.OnLearningAborted += UpdateButtonState;
            _motivation.OnChange += UpdateButtonState;
            
            UpdateButtonState();
        }

        private void UpdateButtonState(int _) => UpdateButtonState();

        private void UpdateButtonState()
        {
            if (_learningController.IsLearning)
            {
                _startLearningButton.interactable = false;
                return;
            }
            
            var isInteractable = _skillsController.HasEmptySocket &&
            _motivation.Amount >= _config.StartLearningMotivationCost;
            
            _startLearningButton.interactable = isInteractable;
        }
    }
}
