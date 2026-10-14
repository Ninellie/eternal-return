using TMPro;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class MessageKeyView : MonoBehaviour
    {
        [SerializeField] private TMP_Text text;
        [SerializeField] private Color normalColor;
        [SerializeField] private Color pressedColor;
        
        public void SetKey(KeyCode key)
        {
            text.text = key.ToString();
        }

        public void Press()
        {
            text.color = pressedColor;
        }

        public void Unpress()
        {
            text.color = normalColor;
        }
    }
}