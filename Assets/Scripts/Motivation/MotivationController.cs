using EternalReturn.Learning;
using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using VContainer.Unity;

namespace EternalReturn.Motivation
{
    public class MotivationController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly ForumController _forumController;
        private readonly MotivationConfig _config;
        private readonly Resource _motivation;
        
        public MotivationController(ForumController forumController,
            LearningController learningController,
            ResourceProvider resourceProvider,
            MotivationConfig config)
        {
            _forumController = forumController;
            _learningController = learningController;
            _motivation = resourceProvider.Motivation;
            _config = config;
        }

        public void Start()
        {
            _learningController.OnLearningStarted += PayLearningMotivation;
            _learningController.OnNodeComplete += PayNodeMotivationOrGainStress;
            _forumController.OnMessageComplete += GainForumMotivation;
        }

        private void PayLearningMotivation()
        {
            _motivation.Decrease(_config.StartLearningMotivationCost);
        }

        private void PayNodeMotivationOrGainStress()
        {
            if (_motivation.Amount >= _config.NodeCompleteMotivationCost)
            {
                _motivation.Decrease(_config.NodeCompleteMotivationCost);
            }
        }
        
        private void GainForumMotivation()
        {
            _motivation.Increase(_config.ForumMotivationGain);
        }
    }
}