using System;
using System.Collections.Generic;
using UnityEngine;

namespace EternalReturn.Topics
{
    public class IntervalSequenceBuilder
    {
        private const int Samples = 256;

        private readonly float _duration;
        private readonly int _messagesAmount;
        private readonly List<Burst> _bursts = new List<Burst>();
        private readonly List<Wave> _waves = new List<Wave>();

        private float _peakPosition = 0.5f;
        private float _sharpness = 4f;
        private float _floor = 0.1f;
        private float _noiseAmplitude;
        private float[] _offsets;

        public IntervalSequenceBuilder(float duration, int messagesAmount)
        {
            _duration = duration;
            _messagesAmount = Mathf.Max(messagesAmount, 1);
        }

        public IntervalSequenceBuilder WithPeak(float position, float sharpness)
        {
            _peakPosition = Mathf.Clamp(position, 0.01f, 0.99f);
            _sharpness = Mathf.Max(sharpness, 0.1f);
            return this;
        }

        public IntervalSequenceBuilder WithFloor(float floor)
        {
            _floor = Mathf.Clamp01(floor);
            return this;
        }

        public IntervalSequenceBuilder WithBurst(float center, float width, float amplitude)
        {
            _bursts.Add(new Burst
            {
                Center = center,
                Width = Mathf.Max(width, 0.001f),
                Amplitude = amplitude
            });
            return this;
        }

        public IntervalSequenceBuilder WithNoise(float amplitude)
        {
            _noiseAmplitude = Mathf.Max(amplitude, 0f);
            return this;
        }

        public IntervalSequenceBuilder WithNoiseWave(float frequency, float phase)
        {
            _waves.Add(new Wave { Frequency = frequency, Phase = phase });
            return this;
        }

        public IntervalSequenceBuilder WithOffsets(float[] offsets)
        {
            _offsets = offsets;
            return this;
        }

        public float[] Build()
        {
            var cdf = BuildCdf();
            var total = cdf[Samples];
            var intervals = new float[_messagesAmount];
            var previousTime = 0f;

            for (var i = 0; i < _messagesAmount; i++)
            {
                var target = (i + GetOffset(i)) / _messagesAmount * total;
                var time = InvertCdf(cdf, target) * _duration;
                intervals[i] = time - previousTime;
                previousTime = time;
            }

            return intervals;
        }

        private float[] BuildCdf()
        {
            var exponentLeft = _peakPosition * _sharpness;
            var exponentRight = (1f - _peakPosition) * _sharpness;
            var peakValue = Mathf.Pow(_peakPosition, exponentLeft) * Mathf.Pow(1f - _peakPosition, exponentRight);

            var cdf = new float[Samples + 1];
            for (var i = 1; i <= Samples; i++)
            {
                var t = (i - 0.5f) / Samples;
                var shape = Mathf.Pow(t, exponentLeft) * Mathf.Pow(1f - t, exponentRight) / peakValue;
                var envelope = _floor + (1f - _floor) * shape;
                var rate = (envelope + EvaluateBursts(t)) * (1f + EvaluateNoise(t));
                cdf[i] = cdf[i - 1] + Mathf.Max(rate, 0f);
            }

            return cdf;
        }

        private float EvaluateBursts(float t)
        {
            var sum = 0f;
            foreach (var burst in _bursts)
            {
                var distance = (t - burst.Center) / burst.Width;
                sum += burst.Amplitude * Mathf.Exp(-0.5f * distance * distance);
            }

            return sum;
        }

        private float EvaluateNoise(float t)
        {
            if (_waves.Count == 0) return 0f;

            var sum = 0f;
            foreach (var wave in _waves)
            {
                sum += Mathf.Sin(Mathf.PI * 2f * wave.Frequency * t + wave.Phase);
            }

            return _noiseAmplitude * sum / _waves.Count;
        }

        private float GetOffset(int index)
        {
            if (_offsets == null || index >= _offsets.Length) return 0.5f;

            return Mathf.Clamp01(_offsets[index]);
        }

        private static float InvertCdf(float[] cdf, float target)
        {
            var index = Array.BinarySearch(cdf, target);
            if (index < 0)
            {
                index = ~index;
            }

            index = Mathf.Clamp(index, 1, Samples);
            var segment = cdf[index] - cdf[index - 1];
            var fraction = segment > 0f ? (target - cdf[index - 1]) / segment : 0f;
            return (index - 1 + fraction) / Samples;
        }

        private struct Burst
        {
            public float Center;
            public float Width;
            public float Amplitude;
        }

        private struct Wave
        {
            public float Frequency;
            public float Phase;
        }
    }
}
