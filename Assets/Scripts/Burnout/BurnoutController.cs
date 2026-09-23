using EternalReturn.Resources_Feature;
using VContainer.Unity;

namespace EternalReturn.Burnout
{
    public class BurnoutController : IStartable
    {
        private const int IncreaseAmount = 1;

        private readonly Resource _stress;
        private readonly Resource _burnout;

        public BurnoutController(ResourceProvider resourceProvider)
        {
            _stress = resourceProvider.Stress;
            _burnout = resourceProvider.Burnout;
        }

        public void Start()
        {
            _stress.OnFill += IncreaseBurnout;
        }

        private void IncreaseBurnout()
        {
            _burnout.Increase(IncreaseAmount);
        }
    }
}
