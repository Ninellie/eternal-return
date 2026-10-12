using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.ViewStyles
{
    [CreateAssetMenu(menuName = "Eternal Return/Button Styles")]
    public class ButtonStyles : ScriptableObject
    {
        [field: SerializeField]
        public ColorBlock Active { get; private set; } = CreateColors(
            normal: new Color(0.95f, 0.72f, 0.28f),
            highlighted: new Color(1f, 0.82f, 0.42f),
            pressed: new Color(0.80f, 0.57f, 0.18f),
            selected: new Color(0.95f, 0.72f, 0.28f),
            disabled: new Color(0.45f, 0.40f, 0.30f, 0.5f));

        [field: SerializeField]
        public ColorBlock Disabled { get; private set; } = CreateColors(
            normal: new Color(0.30f, 0.31f, 0.35f),
            highlighted: new Color(0.30f, 0.31f, 0.35f),
            pressed: new Color(0.30f, 0.31f, 0.35f),
            selected: new Color(0.30f, 0.31f, 0.35f),
            disabled: new Color(0.22f, 0.23f, 0.26f, 0.5f));

        [field: SerializeField]
        public ColorBlock Main { get; private set; } = CreateColors(
            normal: new Color(0.20f, 0.45f, 0.62f),
            highlighted: new Color(0.28f, 0.58f, 0.78f),
            pressed: new Color(0.14f, 0.33f, 0.47f),
            selected: new Color(0.20f, 0.45f, 0.62f),
            disabled: new Color(0.25f, 0.30f, 0.34f, 0.5f));

        private static ColorBlock CreateColors(
            Color normal,
            Color highlighted,
            Color pressed,
            Color selected,
            Color disabled)
        {
            var colors = ColorBlock.defaultColorBlock;
            colors.normalColor = normal;
            colors.highlightedColor = highlighted;
            colors.pressedColor = pressed;
            colors.selectedColor = selected;
            colors.disabledColor = disabled;
            return colors;
        }
    }
}
