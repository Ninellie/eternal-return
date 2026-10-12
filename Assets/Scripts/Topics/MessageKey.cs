using UnityEngine;

namespace EternalReturn.Topics
{
    public class MessageKey
    {
        public MessageKey(KeyCode key)
        {
            Key = key;
        }

        public bool IsPressed { get; set; }
        public KeyCode Key { get; }
    }
}