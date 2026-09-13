using EternalReturn.Resources_Feature;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    public class OverheatController : IStartable, IFixedTickable
    {
        private const int IncreaseAmount = 1;
        private const int DecreaseAmount = 1;
        private const int DecreaseCooldown = 1;

        private readonly Resource _likes;
        private readonly Resource _overheat;
        
        private float _cooldown;

        public OverheatController([Key("likes")] Resource likes, [Key("overheat")] Resource overheat)
        {
            _likes = likes;
            _overheat = overheat;
        }

        public void Start()
        {
            _likes.OnIncrease += IncreaseOverheat;
        }
        
        public void FixedTick()
        {
            if (_overheat.Amount == 0) return;
            
            _cooldown -= Time.fixedDeltaTime;

            if (_cooldown > 0) return;
            
            _overheat.Decrease(DecreaseAmount);
            _cooldown = DecreaseCooldown;
        }

        private void IncreaseOverheat(int _)
        {
            _overheat.Increase(IncreaseAmount);
        }
    }
}