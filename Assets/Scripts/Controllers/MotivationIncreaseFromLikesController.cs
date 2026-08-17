using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class MotivationIncreaseFromLikesController : MonoBehaviour
    {
        [SerializeField] private ResourceRepository resourceRepository;
        
        [SerializeField] private string likeResourceName;
        [SerializeField] private string motivationResourceName;
        
        [SerializeField] private int increaseAmount;
        
        private Resource _likeResource;
        private Resource _motivationResource;
        
        private void OnEnable()
        {
            _likeResource = resourceRepository.GetByName(likeResourceName);
            _motivationResource = resourceRepository.GetByName(motivationResourceName);
            
            _likeResource.OnFill += IncreaseMotivation;
        }

        private void OnDisable()
        {
            _likeResource.OnFill -= IncreaseMotivation;
            _likeResource = null;
            _motivationResource = null;
        }

        private void IncreaseMotivation()
        {
            _motivationResource.Increase(increaseAmount);
        }
    }
}