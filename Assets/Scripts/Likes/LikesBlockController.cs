using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    /// <summary>
    /// Обнуляет и блокирует лайки после перегрева. Снимает блок когда перегрев достигает нуля. 
    /// </summary>
    public class LikesBlockController : IStartable, IFixedTickable
    {
        private const float LikesResettingDuration = 3;
        private const int DecreaseAmount = 1;
        
        private readonly Resource _overheat;
        private readonly Resource _likes;

        private bool _isResettingLikes;
        private float _timePerDecrease;
        private float _decreaseCooldown;

        public LikesBlockController(ResourceProvider resourceProvider)
        {
            _overheat = resourceProvider.Overheat;
            _likes = resourceProvider.Likes;
        }
        
        public void Start()
        {
            _overheat.OnFill += BlockLikes;
            _overheat.OnEmpty += UnblockLikes;
        }

        public void FixedTick()
        {
            if (!_isResettingLikes) return;

            if (_likes.Amount == 0)
            {
                _isResettingLikes = false;
            }
            
            _decreaseCooldown -= Time.fixedDeltaTime;
            
            if (_decreaseCooldown > 0) return;
            
            _likes.Decrease(DecreaseAmount);
            _decreaseCooldown = _timePerDecrease;
        }

        private void UnblockLikes()
        {
            _likes.UnblockIncrease();
        }

        private void BlockLikes()
        {
            _likes.BlockIncrease();
            
            _timePerDecrease = LikesResettingDuration / _likes.Amount;
            _isResettingLikes = true;
        }
    }
}