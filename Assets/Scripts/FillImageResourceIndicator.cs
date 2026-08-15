using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn
{
    public class FillImageResourceIndicator : MonoBehaviour
    {
        [SerializeField] private Image image;

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
            var percent = _resource.Amount / (float)_resource.MaxAmount;
            
            image.rectTransform.anchorMax = new Vector2(percent, 1);
        }
    }
}