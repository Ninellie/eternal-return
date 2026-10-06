using System;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    public class DayCycleController : IStartable
    {
        private static readonly DateTime StartDate = new(2015, 02, 01);
        
        public event Action OnDayStarted;
        public event Action OnDayEnded;
        
        private readonly GameTimeController _gameTimeController;
        
        private bool _wasLateBedtime;
        
        public DayCycleController(GameTimeController gameTimeController)
        {
            _gameTimeController = gameTimeController;
        }

        public void Start()
        {
            _gameTimeController.OnLateBedtime += EndDayLate;
            _gameTimeController.GameTime = StartDate; 
            _gameTimeController.StartNewDay(_wasLateBedtime);
        }

        public void StartDay()
        {
            _gameTimeController.StartNewDay(_wasLateBedtime);
            _wasLateBedtime = false;
            OnDayStarted?.Invoke();
        }

        public void EndDay()
        {
            _gameTimeController.StopTime();
            OnDayEnded?.Invoke();
        }

        private void EndDayLate()
        {
            _wasLateBedtime = true;
            EndDay();
        }
    }
}
