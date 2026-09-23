using EternalReturn.Burnout;
using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    /// <summary>
    /// Обнуляет и блокирует лайки после перегрева. Снимает блок когда перегрев достигает нуля. 
    /// </summary>
    public class LikesBlockController : IStartable, IFixedTickable, ISlowableByBurnout
    {
        private const float Slowdown = 0.05f;
        private const float LikesResettingDuration = 3;
        private const int DecreaseAmount = 1;
        
        private readonly Resource _overheat;
        private readonly Resource _likes;
        private readonly Resource _burnout;

        private bool _isResettingLikes;
        private float _timePerDecrease;
        private float _decreaseCooldown;

        public bool IsSlowed { get; private set; }

        public LikesBlockController(ResourceProvider resourceProvider)
        {
            _overheat = resourceProvider.Overheat;
            _likes = resourceProvider.Likes;
            _burnout = resourceProvider.Burnout;
        }
        
        public void Start()
        {
            _overheat.OnFill += BlockLikes;
            _overheat.OnEmpty += UnblockLikes;
            _burnout.OnFill += Slow;
            _burnout.OnDecrease += _ => Unslow();
        }

        public void FixedTick()
        {
            if (!_isResettingLikes) return;

            var deltaTime = Time.fixedDeltaTime;

            if (IsSlowed)
            {
                deltaTime *= Slowdown;
            }

            if (_likes.Amount == 0)
            {
                _isResettingLikes = false;
            }
            
            _decreaseCooldown -= deltaTime;
            
            if (_decreaseCooldown > 0) return;
            
            _likes.Decrease(DecreaseAmount);
            _decreaseCooldown = _timePerDecrease;
        }

        public void Slow()
        {
            IsSlowed = true;
        }

        public void Unslow()
        {
            IsSlowed = false;
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