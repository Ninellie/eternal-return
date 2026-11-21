using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer;

namespace EternalReturn.Stress
{
    public class BurnoutIndicator : MonoBehaviour
    {
        [SerializeField]
        private H_SDFUIOutline[] indicators;

        [Inject]
        private readonly ResourceProvider _resourceProvider;
        
        private Resource _burnout;
        
        private void Start()
        {
            _resourceProvider.Burnout.OnChange += UpdateIndicator;
            UpdateIndicator(_resourceProvider.Burnout.Amount);
        }

        private void UpdateIndicator(int amount)
        {
            for (var i = 0; i < indicators.Length; i++)
            {
                indicators[i].FillCenter = amount > i;
            }
        }
    }
}