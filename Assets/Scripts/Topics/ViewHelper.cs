using UnityEngine;

namespace EternalReturn.Topics
{
    public static class ViewHelper
    {
        public static void UpdateDurationBar(float baseDuration, float currentDuration, RectTransform durationBar)
        {
            if (baseDuration <= 0f) return;

            var fill = Mathf.Clamp01(currentDuration / baseDuration);
            var anchorMin = durationBar.anchorMin;
            var anchorMax = durationBar.anchorMax;
            anchorMin.x = 0f;
            anchorMax.x = fill;
            durationBar.anchorMin = anchorMin;
            durationBar.anchorMax = anchorMax;
        }
    }
}