using UnityEngine;

namespace EternalReturn.Topics
{
    [CreateAssetMenu(menuName = "Eternal Return/Topic Settings")]
    public class TopicSettings : ScriptableObject
    {
        [Header("Slots")]
        public int SlotsAmount;

        [Header("Topic")]
        [Tooltip("Длительность темы (min, max).")]
        public Vector2 TopicDuration = new Vector2(100f, 200f);
        [Tooltip("Среднее количество сообщений в секунду. Количество сообщений в теме = длительность * это значение.")]
        public float AverageMessagesPerSecond;

        [Header("Messages")]
        [Tooltip("Время, которое живёт сообщение пока не будет сочтено неотвеченным.")]
        public float MessageAnswerTime;
        [Tooltip("Количество символов в сообщении (min, max).")]
        public Vector2 SymbolsAmount = new Vector2(1f, 7f);
        [Tooltip("Символы, из которых случайно составляются сообщения.")]
        public string AllowedSymbols = "abcdefghijklmnopqrstuvwxyz";

        [Header("Intervals")]
        [Tooltip("Пресеты интервалов. На каждую тему выбирается один случайный.")]
        public IntervalsPreset[] Presets;

        [Header("Preview")]
        public AnimationCurve PreviewCurve = new AnimationCurve();

        [ContextMenu("Generate Preview Curve")]
        public void GeneratePreviewCurve()
        {
            const int Windows = 20;

            var generator = new MessageGenerator(this);
            var duration = generator.RollDuration();
            if (duration <= 0f) return;

            var messages = generator.Generate(duration);
            var windowWidth = duration / Windows;
            var counts = new int[Windows];
            var time = 0f;

            foreach (var message in messages)
            {
                time += message.Interval;
                var window = Mathf.Min((int)(time / windowWidth), Windows - 1);
                counts[window]++;
            }

            var curve = new AnimationCurve();
            for (var i = 0; i < Windows; i++)
            {
                curve.AddKey((i + 0.5f) * windowWidth, counts[i] / windowWidth);
            }

            for (var i = 0; i < curve.length; i++)
            {
                curve.SmoothTangents(i, 0f);
            }

            PreviewCurve = curve;

#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        private void OnValidate()
        {
            var minDuration = Mathf.Max(Mathf.Min(TopicDuration.x, TopicDuration.y), 0f);
            var maxDuration = Mathf.Max(TopicDuration.x, TopicDuration.y);
            TopicDuration = new Vector2(minDuration, maxDuration);
            AverageMessagesPerSecond = Mathf.Max(AverageMessagesPerSecond, 0f);

            var minSymbols = Mathf.Max(Mathf.Min(SymbolsAmount.x, SymbolsAmount.y), 1f);
            var maxSymbols = Mathf.Max(SymbolsAmount.x, SymbolsAmount.y, minSymbols);
            SymbolsAmount = new Vector2(minSymbols, maxSymbols);
        }
    }
}
