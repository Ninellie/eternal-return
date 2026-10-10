namespace EternalReturn.Topics
{
    public class TopicMessage
    {
        public float Interval { get; } // Время которое нужно ждать до появления ЭТОГО сообщения. 
        public float Duration { get; set; } // Оставшееся время ожидания ответа на сообщение.
        public float BaseDuration { get; } // Базовое время ожидания ответа на сообщение.
        
        public MessageSymbol[] Symbols { get; }
        public int Pointer { get; set; } // Указатель на текущий символ который игрок должен нажать.

        public TopicMessage(MessageSymbol[] symbols, float interval, float duration)
        {
            Symbols = symbols;
            Pointer = 0;
            Interval = interval;
            BaseDuration = duration;
            Duration = duration;
        }
        
        public void NextSymbol()
        {
            Pointer++;
            Symbols[Pointer].IsPressed = true;
        }
    }
}