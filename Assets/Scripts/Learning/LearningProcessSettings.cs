using UnityEngine;

namespace EternalReturn.Learning
{
    [CreateAssetMenu(menuName = "Eternal Return/Learning Process Settings")]
    public class LearningProcessSettings : ScriptableObject
    {
        [Tooltip("Пресеты генерации. На каждый процесс обучения выбирается один случайный.")]
        public LearningPreset[] Presets;
    }
}
