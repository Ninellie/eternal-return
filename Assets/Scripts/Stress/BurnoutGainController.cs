using EternalReturn.Resources_Feature;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class BurnoutGainController : IStartable
    {
        private readonly Resource _stress;
        private readonly Resource _burnout;

        public BurnoutGainController(ResourceProvider resourceProvider)
        {
            _stress = resourceProvider.Stress;
            _burnout = resourceProvider.Burnout;
        }
        
        public void Start()
        {
            _stress.OnFill += () => _burnout.Increase(1);
        }
    }
}