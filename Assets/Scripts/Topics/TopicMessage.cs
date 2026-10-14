using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicMessage
    {
        public event Action<int> OnNextKeyPressed;
        public event Action OnKeysReset;
        
        public float Interval { get; } // Время которое нужно ждать до появления ЭТОГО сообщения. 
        public float Duration { get; set; } // Оставшееся время ожидания ответа на сообщение.
        public float BaseDuration { get; } // Базовое время ожидания ответа на сообщение.
        
        public MessageKey[] Keys { get; }
        public int Pointer { get; private set; } // Указатель на текущий символ который игрок должен нажать.
        
        private MessageKey NextKey => Keys[Pointer];
        
        public TopicMessage(MessageKey[] keys, float interval, float duration)
        {
            Keys = keys;
            Pointer = 0;
            Interval = interval;
            BaseDuration = duration;
            Duration = duration;
        }

        /// <summary>
        /// Пытается нажать следующий символ. Если переданный символ не является следующим - сбрасывает сообщение.
        /// </summary>
        public void TryPressKey(KeyCode key)
        {
            if (NextKey.Key == key)
            {
                NextKey.IsPressed = true;
                OnNextKeyPressed?.Invoke(Pointer);
                Pointer++;
                return;
            }
            
            Pointer = 0;
            
            foreach (var symbol in Keys)
            {
                symbol.IsPressed = false;
            }
            
            OnKeysReset?.Invoke();
        }

        public bool IsCompleted()
        {
            return Pointer >= Keys.Length;
        }
    }
}