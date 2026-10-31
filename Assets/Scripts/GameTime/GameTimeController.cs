using System;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    // Класс занимается обработкой времени и является издателем временных событий
    public class GameTimeController : IFixedTickable
    {
        private static readonly TimeSpan Midnight = new(0, 0, 0);
        public DateTime GameTime { get; set; }
        
        public event Action OnBedtime;
        public event Action OnLateBedtime;

        private readonly GameTimeSettings _settings;
        
        private bool _isRunning;
        private bool _midnightWasReached;
        
        public GameTimeController(GameTimeSettings settings)
        {
            _settings = settings;
        }

        public int GetDay()
        {
            return GameTime.Day;
        }
        
        public void FixedTick()
        {
            if (!_isRunning) return;
            
            var next = GameTime.AddMinutes(Time.fixedDeltaTime * _settings.GetMinutesPerRealSecond());
            
            if (!_midnightWasReached)
            {
                _midnightWasReached = HasReached(GameTime.TimeOfDay, next.TimeOfDay, Midnight);
            }
            
            if (HasReached(GameTime.TimeOfDay, next.TimeOfDay, _settings.NormalBedtime.ToTimeSpan()))
            {
                OnBedtime?.Invoke();
            }

            if (HasReached(GameTime.TimeOfDay, next.TimeOfDay, _settings.LateBedtime.ToTimeSpan()))
            { 
                _isRunning = false;
                OnLateBedtime?.Invoke();
            }
            
            
            GameTime = next;
        }

        public void StopTime()
        {
            _isRunning = false;
        }

        public void StartNewDay(bool wasLateBedtime)
        {
            // Если вчера уже настал новый день
            var day = _midnightWasReached ? GameTime.Day : GameTime.Day + 1;
            // Если герой заснул поздно, то и проснётся позже. 
            var time = wasLateBedtime ? _settings.LateWakeTime.ToTimeSpan() : _settings.NormalWakeTime.ToTimeSpan();
            SetMorningTime(day, time);
            _isRunning = true;
            _midnightWasReached = false;
        }

        private void SetMorningTime(int day, TimeSpan wakeTime)
        {
            GameTime = new DateTime(
                GameTime.Year, 
                GameTime.Month,
                day,
                wakeTime.Hours,
                wakeTime.Minutes,
                wakeTime.Seconds);
        }

        // Пересекло ли на этом кадре игровое время отметку target?
        private static bool HasReached(TimeSpan current, TimeSpan next, TimeSpan target)
        {
            // Следующее время не меньше текущего - тик остался внутри одних суток.
            if (current <= next)
            {
                return current < target // До тика цель была строго впереди.
                       && next >= target; // Этот тик дошёл до цели или перешагнул её.
            }

            // next меньше current - тик перешёл через полночь.
            return current < target // Цель ещё впереди и лежит до полуночи.
                   || next >= target; // Цель лежит после полуночи и не дальше next.
        }
    } 
}
