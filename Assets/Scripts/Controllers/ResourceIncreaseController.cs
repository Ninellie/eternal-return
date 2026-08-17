using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class ResourceIncreaseController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private ResourceRepository resourceRepository;
        
        [Header("Settings")]
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

        public void Increase()
        {
            _resource.Increase(increaseAmount);
        }
    }
}