using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressBarViewController : ITickable
    {
        private readonly RectTransform _stressBar;
        private readonly Resource _stress;

        public StressBarViewController(
            [Key("stress-bar")] RectTransform stressBar, ResourceProvider resourceProvider)
        {
            _stressBar = stressBar;
            _stress = resourceProvider.Stress;
        }
        
        public void Tick()
        {
            UpdateDurationBar();
        }
        
        private void UpdateDurationBar()
        {
            ViewHelper.UpdateDurationBar(
                _stress.MaxAmount,
                _stress.Amount,
                _stressBar,
                ViewHelper.DurationBarMode.RightToLeft);
        }
    }
}