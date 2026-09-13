using TMPro;
using UnityEngine;

namespace EternalReturn.Resources_Feature
{
    public class AmountTextResourceIndicatorView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI text;

        private Resource _resource;

        public void SetResource(Resource resource)
        {
            _resource = resource;
        }
        
        private void OnEnable()
        {
            UpdateIndicator(_resource.Amount);
            _resource.OnChange += UpdateIndicator;
        }

        private void OnDisable()
        {
            _resource.OnChange -= UpdateIndicator;
            _resource = null;
        }

        private void UpdateIndicator(int amount)
        {
            text.text = amount.ToString();
        }
    }
}