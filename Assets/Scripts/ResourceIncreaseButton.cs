using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn
{
    public class ResourceIncreaseButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [SerializeField] private ResourceRepository resourceRepository;
        [SerializeField] private string resourceName;
        [SerializeField] private int increaseAmount;

        private Resource _resource;

        private void OnEnable()
        {
            _resource = resourceRepository.GetByName(resourceName);
        }

        private void OnDisable()
        {
            _resource = null;
        }

        public void Like()
        {
            _resource.Increase(increaseAmount);
        }
    }
}