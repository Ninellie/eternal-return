using System;
using UnityEngine;

namespace EternalReturn
{
    [Serializable]
    public class Resource
    {
        [SerializeField] private string name;
        
        [SerializeField] private int amount;
        [SerializeField] private int maxAmount;
        [SerializeField] private bool emptyOnFill;
        [SerializeField] private bool isIncreaseBlocked;
        
        public string Name => name;
        
        public int Amount => amount;
        public int MaxAmount => maxAmount;
        public bool IsIncreaseBlocked => isIncreaseBlocked;
        
        public event Action<int> OnChange;
        public event Action<int> OnIncrease;
        public event Action<int> OnDecrease;
        public event Action OnFill;
        public event Action OnEmpty;
        public event Action OnBlocked;
        public event Action OnUnblocked;
        
        public void Increase(int value)
        {
            if (value == 0) return;
            if (isIncreaseBlocked) return;
            
            amount += value;
            
            amount = Mathf.Clamp(amount, 0, MaxAmount);
            
            if (Amount == MaxAmount)
            {
                OnFill?.Invoke();
                
                if (emptyOnFill)
                {
                    amount = 0;
                    OnEmpty?.Invoke();
                }
            }
            
            OnIncrease?.Invoke(amount);
            OnChange?.Invoke(Amount);
        }

        public void Decrease(int value)
        {
            if (value == 0) return;
            
            amount -= value;
            
            amount = Mathf.Clamp(amount, 0, MaxAmount);

            if (amount == 0)
            {
                OnEmpty?.Invoke();
            }
            
            OnDecrease?.Invoke(amount);
            OnChange?.Invoke(Amount);
        }

        public void BlockIncrease()
        {
            isIncreaseBlocked = true;
            OnBlocked?.Invoke();
        }

        public void UnblockIncrease()
        {
            isIncreaseBlocked = false;
            OnUnblocked?.Invoke();
        }
    }
}