using EternalReturn.Resources_Feature;
using EternalReturn.Stress.Mini_Game;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class BurnoutGainController : IStartable
    {
        private readonly StressGameController _stressGameController;
        private readonly Resource _burnout;

        public BurnoutGainController(StressGameController stressGameController, ResourceProvider resourceProvider)
        {
            _stressGameController = stressGameController;
            _burnout = resourceProvider.Burnout;
        }
        
        public void Start()
        {
            _stressGameController.OnGameLost += () => _burnout.Increase(1);
        }
    }
}
