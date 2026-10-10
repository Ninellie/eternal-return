using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EternalReturn.Topics
{
    public class MessageGenerator
    {
        private readonly TopicSettings _settings;

        public MessageGenerator(TopicSettings settings)
        {
            _settings = settings;
        }

        public float RollDuration()
        {
            return Roll(_settings.TopicDuration);
        }

        public Queue<TopicMessage> Generate(float duration)
        {
            var intervals = GenerateIntervals(duration);
            var messages = new Queue<TopicMessage>(intervals.Length);

            foreach (var interval in intervals)
            {
                messages.Enqueue(new TopicMessage(CreateSymbols(), interval, _settings.MessageAnswerTime));
            }

            return messages;
        }

        private MessageSymbol[] CreateSymbols()
        {
            var allowedSymbols = _settings.AllowedSymbols;
            if (string.IsNullOrEmpty(allowedSymbols))
            {
                throw new InvalidOperationException("TopicSettings has no allowed symbols.");
            }

            var amount = RollSymbolsAmount();
            var symbols = new MessageSymbol[amount];
            for (var i = 0; i < amount; i++)
            {
                symbols[i] = new MessageSymbol { Text = allowedSymbols[Random.Range(0, allowedSymbols.Length)] };
            }

            return symbols;
        }

        private int RollSymbolsAmount()
        {
            var range = _settings.SymbolsAmount;
            var min = Mathf.RoundToInt(range.x);
            var max = Mathf.RoundToInt(range.y);
            var amount = Mathf.FloorToInt(Roll(new Vector2(min, max + 1)));
            return Mathf.Clamp(amount, min, max);
        }

        public float[] GenerateIntervals(float duration)
        {
            var preset = RollPreset();
            var amount = Mathf.Max(1, Mathf.RoundToInt(duration * _settings.AverageMessagesPerSecond));

            var builder = new IntervalSequenceBuilder(duration, amount)
                .WithPeak(Roll(preset.PeakPosition), Roll(preset.Sharpness))
                .WithFloor(preset.Floor)
                .WithNoise(preset.NoiseAmplitude)
                .WithNoiseWave(Roll(preset.NoiseLowFrequency), RollPhase())
                .WithNoiseWave(Roll(preset.NoiseHighFrequency), RollPhase())
                .WithOffsets(RollOffsets(amount, preset.OffsetsJitter));

            var burstsAmount = Random.Range(preset.BurstsAmount.x, preset.BurstsAmount.y + 1);
            for (var i = 0; i < burstsAmount; i++)
            {
                builder.WithBurst(Roll(preset.BurstCenter), Roll(preset.BurstWidth), Roll(preset.BurstAmplitude));
            }

            return builder.Build();
        }

        private IntervalsPreset RollPreset()
        {
            var presets = _settings.Presets;
            if (presets == null || presets.Length == 0)
            {
                throw new InvalidOperationException("TopicSettings has no intervals presets.");
            }

            return presets[Random.Range(0, presets.Length)];
        }

        private static float Roll(Vector2 range)
        {
            return Random.Range(range.x, range.y);
        }

        private static float RollPhase()
        {
            return Random.Range(0f, Mathf.PI * 2f);
        }

        private static float[] RollOffsets(int amount, float jitter)
        {
            var offsets = new float[amount];
            for (var i = 0; i < amount; i++)
            {
                offsets[i] = 0.5f + (Random.value - 0.5f) * jitter;
            }

            return offsets;
        }
    }
}
