using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressBarViewController : IStartable
    {
        private readonly RectTransform _bar;
        private readonly Resource _stress;

        public StressBarViewController(
            [Key("stress-bar")] RectTransform bar, ResourceProvider resourceProvider)
        {
            _bar = bar;
            _stress = resourceProvider.Stress;
        }

        public void Start()
        {
            _stress.OnChange += UpdateDurationBar;
            UpdateDurationBar(_stress.Amount);
        }

        private void UpdateDurationBar(int amount)
        {
            ViewHelper.UpdateDurationBar(
                _stress.MaxAmount,
                amount,
                _bar,
                ViewHelper.DurationBarMode.RightToLeft);
        }
    }
}