using UnityEngine;

namespace EternalReturn.Learning
{
    [CreateAssetMenu(menuName = "Eternal Return/Learning Preset")]
    public class LearningPreset : ScriptableObject
    {
        [Min(0.1f)]
        [Tooltip("Длительность всего процесса обучения в секундах.")]
        public float ProcessDuration = 30f;

        [Min(1)]
        [Tooltip("Количество узлов, которые появятся за время процесса.")]
        public int NodesAmount = 100;

        [Min(0.1f)]
        [Tooltip("Время жизни одного узла в секундах.")]
        public float NodeDuration = 3f;

        [Min(1f)]
        [Tooltip("Радиус узла в пикселях.")]
        public float NodeRadius = 40f;
    }
}
