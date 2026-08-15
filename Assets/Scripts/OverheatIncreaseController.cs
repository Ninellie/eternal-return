using UnityEngine;

namespace EternalReturn
{
    public class OverheatIncreaseController : MonoBehaviour
    {
        [SerializeField] private ResourceRepository resourceRepository;
        
        [SerializeField] private string likeResourceName;
        [SerializeField] private string overheatResourceName;
        
        [SerializeField] private int increaseAmount;
        
        private Resource _likeResource;
        private Resource _overheatResource;
        
        private void OnEnable()
        {
            _likeResource = resourceRepository.GetByName(likeResourceName);
            _overheatResource = resourceRepository.GetByName(overheatResourceName);
            
            _likeResource.OnIncrease += IncreaseOverheat;
        }

        private void OnDisable()
        {
            _likeResource.OnIncrease -= IncreaseOverheat;
            _likeResource = null;
            _overheatResource = null;
        }

        private void IncreaseOverheat(int _)
        {
            _overheatResource.Increase(increaseAmount);
        }
    }
}