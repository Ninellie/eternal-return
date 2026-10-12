using System;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class TopicMessage
    {
        public event Action OnKeyPressed;
        public event Action OnMessageReset;
        
        public float Interval { get; } // Время которое нужно ждать до появления ЭТОГО сообщения. 
        public float Duration { get; set; } // Оставшееся время ожидания ответа на сообщение.
        public float BaseDuration { get; } // Базовое время ожидания ответа на сообщение.
        
        public MessageKey[] Keys { get; }
        
        private MessageKey NextKey => Keys[_pointer];
        
        private int _pointer; // Указатель на текущий символ который игрок должен нажать.
        
        public TopicMessage(MessageKey[] keys, float interval, float duration)
        {
            Keys = keys;
            _pointer = 0;
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
                _pointer++;

                OnKeyPressed?.Invoke();
                return;
            }
            
            _pointer = 0;
            
            foreach (var symbol in Keys)
            {
                symbol.IsPressed = false;
            }
            
            OnMessageReset?.Invoke();
        }

        public bool IsCompleted()
        {
            return _pointer >= Keys.Length;
        }
    }
}