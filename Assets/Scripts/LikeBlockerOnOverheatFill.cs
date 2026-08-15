using UnityEngine;

namespace EternalReturn
{
    public class LikeBlockerOnOverheatFill : MonoBehaviour
    {
        [SerializeField] private ResourceRepository resourceRepository;
        
        [SerializeField] private string overheatResourceName;
        [SerializeField] private string likeResourceName;

        private Resource _overheatResource;
        private Resource _likeResource;
        
        private void OnEnable()
        {
            _overheatResource = resourceRepository.GetByName(overheatResourceName);
            _likeResource = resourceRepository.GetByName(likeResourceName);
            
            _overheatResource.OnFill += _likeResource.Block;
            _overheatResource.OnEmpty += _likeResource.Unblock;
        }
        
        private void OnDisable()
        {
            _overheatResource.OnFill -= _likeResource.Block;
            _overheatResource.OnEmpty -= _likeResource.Unblock;
            
            _overheatResource = null;
            _likeResource = null;
        }
    }
}