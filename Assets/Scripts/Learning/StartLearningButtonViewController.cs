using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Learning
{
    public class StartLearningButtonViewController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly Button _startLearningButton;

        public StartLearningButtonViewController(LearningController learningController,
            [Key("start-learning")] Button startLearningButton)
        {
            _learningController = learningController;
            _startLearningButton = startLearningButton;
        }

        public void Start()
        {
            _startLearningButton.onClick.AddListener(_learningController.StartLearning);
        }
    }
}
