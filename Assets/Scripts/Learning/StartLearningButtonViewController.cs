using EternalReturn.Skills;
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

        public StartLearningButtonViewController(
            LearningController learningController,
            SkillsController skillsController,
            [Key("start-learning")] Button startLearningButton)
        {
            _learningController = learningController;
            _skillsController = skillsController;
            _startLearningButton = startLearningButton;
        }

        public void Start()
        {
            _startLearningButton.onClick.AddListener(_learningController.OpenSkillChoice);

            _skillsController.OnSocketsChanged += UpdateButtonState;
            _learningController.OnSkillChoiceOpened += UpdateButtonState;
            _learningController.OnSkillChoiceClosed += UpdateButtonState;
            _learningController.OnLearningStarted += UpdateButtonState;
            _learningController.OnLearningFinished += UpdateButtonState;

            UpdateButtonState();
        }

        private void UpdateButtonState()
        {
            var isInteractable = _skillsController.HasEmptySocket
                && !_learningController.IsChoosingSkill
                && !_learningController.IsLearning;
            _startLearningButton.interactable = isInteractable;
        }
    }
}
