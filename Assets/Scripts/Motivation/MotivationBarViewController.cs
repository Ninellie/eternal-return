using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using UnityEngine;
using VContainer;

namespace EternalReturn.Motivation
{
    public class MotivationBarViewController : MonoBehaviour
    {
        [SerializeField]
        private RectTransform bar;
        
        [Inject]
        private ResourceProvider _resourceProvider;

        private Resource _motivation;
        
        private void Start()
        {
            _motivation = _resourceProvider.Motivation;
            
            _motivation.OnChange += UpdateIndicator;
            
            UpdateIndicator(_motivation.Amount);
        }
        private void UpdateIndicator(int amount)
        {
            ViewHelper.UpdateDurationBar(_motivation.MaxAmount, amount, bar,
                ViewHelper.DurationBarMode.RightToLeft);
        }
    }
}