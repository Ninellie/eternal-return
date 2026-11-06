using EternalReturn.Learning;
using EternalReturn.Resources_Feature;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressGainController : IStartable
    {
        private readonly LearningController _learningController;
        private readonly StressConfig _stressConfig;
        private readonly Resource _stress;
        
        public StressGainController(LearningController learningController, ResourceProvider resourceProvider, StressConfig stressConfig)
        {
            _learningController = learningController;
            _stress = resourceProvider.Stress;
            _stressConfig = stressConfig;
        }

        public void Start()
        {
            _learningController.OnNodeLost += GainLearningStress;
        }

        private void GainLearningStress()
        {
            _stress.Increase(_stressConfig.learningStressGain);
        }
    }
}