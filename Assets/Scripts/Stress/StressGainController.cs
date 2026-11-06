using EternalReturn.Learning;
using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressGainController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly ForumController _forumController;
        private readonly StressConfig _stressConfig;
        private readonly Resource _stress;
        
        public StressGainController(LearningController learningController,
            ResourceProvider resourceProvider,
            StressConfig stressConfig,
            ForumController forumController)
        {
            _learningController = learningController;
            _stress = resourceProvider.Stress;
            _stressConfig = stressConfig;
            _forumController = forumController;
        }

        public void Start()
        {
            _learningController.OnNodeLost += GainLearningStress;
            _forumController.OnMessageLost += GainForumStress;
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