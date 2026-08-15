using TMPro;
using UnityEngine;

namespace EternalReturn
{
    public class AmountTextResourceIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        
        [SerializeField] private ResourceRepository resourceRepository;
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