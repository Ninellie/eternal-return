using EternalReturn.Core;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Controllers
{
    public class LikesBlockController : MonoBehaviour
    {
        [SerializeField] private ResourceRepository resourceRepository;
        [SerializeField] private Button likeButton;
        [SerializeField] private string overheatResourceName;
        [SerializeField] private string likeResourceName;
        [SerializeField] private float likesNullTime;

        private Resource _overheatResource;
        private Resource _likeResource;

        private bool _isResettingLikes;
        private float _timePerDecrease;
        private float _decreaseCooldown;
        
        private void OnEnable()
        {
            _overheatResource = resourceRepository.GetByName(overheatResourceName);
            _likeResource = resourceRepository.GetByName(likeResourceName);
            
            _overheatResource.OnFill += BlockLikes;
            _overheatResource.OnEmpty += UnblockLikes;
        }
        
        private void OnDisable()
        {
            _overheatResource.OnFill -= BlockLikes;
            _overheatResource.OnEmpty -= UnblockLikes;
            
            _overheatResource = null;
            _likeResource = null;
        }

        private void FixedUpdate()
        {
            if (!_isResettingLikes) return;

            if (_likeResource.Amount == 0)
            {
                _isResettingLikes = false;
            }
            
            _decreaseCooldown -= Time.fixedDeltaTime;
            
            if (_decreaseCooldown > 0) return;
            
            _likeResource.Decrease(1);
            _decreaseCooldown = _timePerDecrease;
        }

        private void BlockLikes()
        {
            likeButton.interactable = false;
            _likeResource.BlockIncrease();
            
            _timePerDecrease = likesNullTime / _likeResource.Amount;
            _isResettingLikes = true;
        }

        private void UnblockLikes()
        {
            _likeResource.UnblockIncrease();
            likeButton.interactable = true;
        }
    }
}