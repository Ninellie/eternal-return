using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.ViewStyles
{
    [CreateAssetMenu(menuName = "Eternal Return/Style/Button Style")]
    public class ButtonStyle : ScriptableObject
    {
        [SerializeField] private ColorStyle normal;
        [SerializeField] private ColorStyle highlighted;
        [SerializeField] private ColorStyle pressed;
        [SerializeField] private ColorStyle selected;
        [SerializeField] private ColorStyle disabled;
        [SerializeField] [Range(1, 5)] private float colorMultiplier = 1f;
        [SerializeField] private float fadeDuration = 0.1f;

        public ColorBlock ColorBlock => new()
        {
            colorMultiplier = colorMultiplier,
            fadeDuration = fadeDuration,
            normalColor = normal.Color,
            highlightedColor = highlighted.Color,
            pressedColor = pressed.Color,
            selectedColor = selected.Color,
            disabledColor = disabled.Color
        };
    }
}