using System;

namespace EternalReturn
{
    public class LikeResource
    {
        public int Amount { get; private set; }

        public int MaxAmount => 40;

        public event Action<int> OnChange;
        
        public void Increase()
        {
            Amount += 1;

            if (Amount == MaxAmount)
            {
                Amount = 0;
            }
            
            OnChange!.Invoke(Amount);
        }
    }
}