using UnityEngine.UI;

namespace EternalReturn.ViewStyles
{
    public static class ViewStyleHelper
    {
        public static void SetColors(Button button, ColorBlock colors)
        {
            button.transition = Selectable.Transition.ColorTint;
            button.colors = colors;
        }
    }
}
