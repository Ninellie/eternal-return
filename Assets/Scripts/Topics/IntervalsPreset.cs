using UnityEngine;

namespace EternalReturn.Topics
{
    [CreateAssetMenu(menuName = "Eternal Return/Intervals Preset")]
    public class IntervalsPreset : ScriptableObject
    {
        [Tooltip("Положение пика в долях длительности темы (min, max).")]
        public Vector2 PeakPosition = new Vector2(0.1f, 0.9f);
        [Tooltip("Острота пика (min, max). Чем больше, тем уже пик.")]
        public Vector2 Sharpness = new Vector2(2f, 8f);
        [Range(0f, 1f)]
        [Tooltip("Минимальный уровень скорости относительно пика.")]
        public float Floor = 0.1f;

        [Tooltip("Количество всплесков (min, max).")]
        public Vector2Int BurstsAmount = new Vector2Int(0, 2);
        [Tooltip("Положение центра всплеска в долях длительности темы (min, max).")]
        public Vector2 BurstCenter = new Vector2(0f, 1f);
        [Tooltip("Амплитуда всплеска относительно пика (min, max).")]
        public Vector2 BurstAmplitude = new Vector2(0.3f, 1f);
        [Tooltip("Ширина всплеска в долях длительности темы (min, max).")]
        public Vector2 BurstWidth = new Vector2(0.03f, 0.1f);

        [Min(0f)]
        public float NoiseAmplitude = 0.15f;
        [Tooltip("Частота медленной составляющей шума (min, max).")]
        public Vector2 NoiseLowFrequency = new Vector2(1f, 3f);
        [Tooltip("Частота быстрой составляющей шума (min, max).")]
        public Vector2 NoiseHighFrequency = new Vector2(4f, 8f);

        [Range(0f, 1f)]
        [Tooltip("Разброс положения сообщения внутри своей доли. 0 - ровно по кривой, 1 - любое место доли.")]
        public float OffsetsJitter = 0.5f;

        private void OnValidate()
        {
            PeakPosition = OrderRange(PeakPosition);
            Sharpness = OrderRange(Sharpness);
            BurstCenter = OrderRange(BurstCenter);
            BurstAmplitude = OrderRange(BurstAmplitude);
            BurstWidth = OrderRange(BurstWidth);
            NoiseLowFrequency = OrderRange(NoiseLowFrequency);
            NoiseHighFrequency = OrderRange(NoiseHighFrequency);
            BurstsAmount = new Vector2Int(
                Mathf.Max(BurstsAmount.x, 0),
                Mathf.Max(BurstsAmount.y, BurstsAmount.x));
        }

        private static Vector2 OrderRange(Vector2 range)
        {
            return new Vector2(Mathf.Min(range.x, range.y), Mathf.Max(range.x, range.y));
        }
    }
}
