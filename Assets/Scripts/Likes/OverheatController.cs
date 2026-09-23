using EternalReturn.Burnout;
using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    public class OverheatController : IStartable, IFixedTickable, ISlowableByBurnout
    {
        private const float Slowdown = 0.05f;
        private const int IncreaseAmount = 1;
        private const int DecreaseAmount = 1;
        private const int DecreaseCooldown = 1;

        private readonly Resource _likes;
        private readonly Resource _overheat;
        private readonly Resource _burnout;
        
        private float _cooldown;

        public bool IsSlowed { get; private set; }

        public OverheatController(ResourceProvider resourceProvider)
        {
            _likes = resourceProvider.Likes;
            _overheat = resourceProvider.Overheat;
            _burnout = resourceProvider.Burnout;
        }

        public void Start()
        {
            _likes.OnIncrease += IncreaseOverheat;
            _burnout.OnFill += Slow;
            _burnout.OnDecrease += _ => Unslow();
        }

        public void FixedTick()
        {
            if (_overheat.Amount == 0) return;

            var deltaTime = Time.fixedDeltaTime;

            if (IsSlowed)
            {
                deltaTime *= Slowdown;
            }
            
            _cooldown -= deltaTime;

            if (_cooldown > 0) return;
            
            _overheat.Decrease(DecreaseAmount);
            _cooldown = DecreaseCooldown;
        }

        public void Slow()
        {
            IsSlowed = true;
        }

        public void Unslow()
        {
            IsSlowed = false;
        }

        private void IncreaseOverheat(int _)
        {
            _overheat.Increase(IncreaseAmount);
        }
    }
}