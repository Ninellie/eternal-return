using EternalReturn.Core;
using TMPro;
using UnityEngine;

namespace EternalReturn.View
{
    public class AmountTextResourceIndicatorView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private ResourceRepository resourceRepository;
        [SerializeField] private TextMeshProUGUI text;
        
        [Header("Settings")]
        [SerializeField] private string resourceName;

        private Resource _resource;
        
        private void OnEnable()
        {
            _resource = resourceRepository.GetByName(resourceName);
            
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