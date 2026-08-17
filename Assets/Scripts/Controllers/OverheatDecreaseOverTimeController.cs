using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class OverheatDecreaseOverTimeController : MonoBehaviour
    {
        [SerializeField] private ResourceRepository resourceRepository;
        
        [SerializeField] private string overheatResourceName;
        
        [SerializeField] private int decreaseAmount;
        [SerializeField] private float decreaseCooldown;
        
        private Resource _overheatResource;
        private float _cooldown;

        private void OnEnable()
        {
            _overheatResource = resourceRepository.GetByName(overheatResourceName);
        }

        private void OnDisable()
        {
            _overheatResource = null;
        }
        
        private void FixedUpdate()
        {
            if (_overheatResource.Amount == 0) return;
            
            _cooldown -= Time.fixedDeltaTime;

            if (_cooldown > 0) return;
            
            _overheatResource.Decrease(decreaseAmount);
            _cooldown = decreaseCooldown;
        }
    }
}