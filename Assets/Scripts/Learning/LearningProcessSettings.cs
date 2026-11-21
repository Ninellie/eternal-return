using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Learning
{
    [CreateAssetMenu(menuName = "Eternal Return/Learning Process Settings")]
    public class LearningProcessSettings : ScriptableObject
    {
        [Tooltip("Пресеты генерации. На каждый процесс обучения выбирается один случайный.")]
        [SerializeField] private LearningPreset[] presets;

        public LearningPreset[] Presets => presets;
    }
}
