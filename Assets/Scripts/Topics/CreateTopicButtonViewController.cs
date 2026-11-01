using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Topics
{
    public class CreateTopicButtonViewController : IStartable
    {
        private readonly ForumController _forumController;
        private readonly Button _createTopicButton;
        
        public CreateTopicButtonViewController(ForumController forumController,
            [Key("create-topic")] Button createTopicButton)
        {
            _forumController = forumController;
            _createTopicButton = createTopicButton;
        }
        
        public void Start()
        {
            Debug.Log("Creating topic button");
            _createTopicButton.onClick.AddListener(_forumController.CreateTopic);

            _forumController.OnTopicClosed += UpdateButtonState;
            _forumController.OnTopicAborted += UpdateButtonState;
            _forumController.OnTopicOpened += UpdateButtonState;
        }

        private void UpdateButtonState(TopicSlot _) => UpdateButtonState();

        private void UpdateButtonState(Topic _) => UpdateButtonState();

        private void UpdateButtonState()
        {
            _createTopicButton.interactable = _forumController.HasUnoccupiedSlot;
            
        }
    }
}