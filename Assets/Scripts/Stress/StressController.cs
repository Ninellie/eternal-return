using EternalReturn.Learning;
using EternalReturn.Motivation;
using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly ForumController _forumController;
        private readonly StressConfig _stressConfig;
        private readonly MotivationConfig _motivationConfig;
        private readonly Resource _stress;
        private readonly Resource _motivation;
        
        public StressController(LearningController learningController,
            ForumController forumController, 
            StressConfig stressConfig,
            MotivationConfig motivationConfig,
            ResourceProvider resourceProvider)
        {
            _learningController = learningController;
            _forumController = forumController;
            _stressConfig = stressConfig;
            _motivationConfig = motivationConfig;
            _stress = resourceProvider.Stress;
            _motivation = resourceProvider.Motivation;
        }

        public void Start()
        {
            _learningController.OnNodeComplete += GainNodeStress;
            _learningController.OnNodeLost += GainLearningStress;
            _forumController.OnMessageLost += GainForumStress;
        }

        private void GainNodeStress()
        {
            if (_motivation.Amount < _motivationConfig.NodeCompleteMotivationCost)
            {
                _stress.Increase(_motivationConfig.NodeCompleteStressGain);
            }
        }

        private void GainLearningStress()
        {
            _stress.Increase(_stressConfig.learningStressGain);
        }
        
        private void GainForumStress()
        {
            _stress.Increase(_stressConfig.forumStressGain);
        }
    }
}