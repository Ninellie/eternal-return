using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Topics
{
    public static class ViewHelper
    {
        public enum DurationBarMode
        {
            LeftToRight,
            RightToLeft,
            TopToBottom,
            BottomToTop
        }
        
        public static void UpdateDurationBar(float baseDuration, float currentDuration, RectTransform durationBar, DurationBarMode mode)
        {
            if (baseDuration <= 0f) return;

            var fill = Mathf.Clamp01(currentDuration / baseDuration);
            var anchorMin = durationBar.anchorMin;
            var anchorMax = durationBar.anchorMax;

            switch (mode)
            {
                case DurationBarMode.LeftToRight:
                    anchorMin.x = 1f - fill;
                    anchorMax.x = 1f;
                    break;
                case DurationBarMode.RightToLeft:
                    anchorMin.x = 0f;
                    anchorMax.x = fill;
                    break;
                case DurationBarMode.TopToBottom:
                    anchorMin.y = 0f;
                    anchorMax.y = fill;
                    break;
                case DurationBarMode.BottomToTop:
                    anchorMin.y = 1f - fill;
                    anchorMax.y = 1f;
                    break;
            }

            durationBar.anchorMin = anchorMin;
            durationBar.anchorMax = anchorMax;
        }
        
        public static void SetColors(Button button, ColorBlock colors)
        {
            button.transition = Selectable.Transition.ColorTint;
            button.colors = colors;
        }
    }
}