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
            _createTopicButton.onClick.AddListener(_forumController.CreateTopic);
        }
    }
}