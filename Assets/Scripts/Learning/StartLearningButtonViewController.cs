using EternalReturn.Topics;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class StartLearningButtonViewController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly ForumController _forumController;
        private readonly Button _startLearningButton;

        public StartLearningButtonViewController(
            LearningController learningController,
            ForumController forumController,
            [Key("start-learning")] Button startLearningButton)
        {
            _learningController = learningController;
            _forumController = forumController;
            _startLearningButton = startLearningButton;
        }

        public void Start()
        {
            _startLearningButton.onClick.AddListener(_learningController.StartLearning);

            _forumController.OnTopicOpened += OnTopicOpened;
            _forumController.OnTopicClosed += OnTopicClosed;
            _learningController.OnLearningStarted += UpdateButtonState;
            _learningController.OnLearningFinished += UpdateButtonState;

            UpdateButtonState();
        }

        private void OnTopicOpened(TopicSlot _) => UpdateButtonState();

        private void OnTopicClosed(Topic _) => UpdateButtonState();

        private void UpdateButtonState()
        {
            var isInteractable = _forumController.HasUnoccupiedSlot && !_learningController.IsLearning;
            SetInteractable(isInteractable);
        }

        private void SetInteractable(bool isInteractable)
        {
            _startLearningButton.interactable = isInteractable;
        }
    }
}
