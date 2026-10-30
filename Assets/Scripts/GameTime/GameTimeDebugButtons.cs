using UnityEngine;
using VContainer;

namespace EternalReturn.GameTime
{ 
    public class GameTimeDebugButtons : MonoBehaviour
    {
        private enum FastTimeMode
        {
            ToBadTime,
            ToLateBadTime
        }
        
        [Inject] private readonly GameTimeController _gameTimeController;
        [Inject] private readonly GameTimeSettings _settings;

        private FastTimeMode _mode;

        private void OnGUI()
        {
            if(GUI.Button(new Rect(120, 40,80,20), "Ускорить до времени спать"))
            {
                _gameTimeController.OnBedtime += StopAccelerationToBadTime;
                _mode = FastTimeMode.ToBadTime;
                _settings.IsAccelerationEnabled = true;
            }
            
            if(GUI.Button(new Rect(120, 80,80,20), "Ускорить до завершения дня"))
            {
                _gameTimeController.OnLateBedtime += StopAccelerationToLateBadTime;
                _mode = FastTimeMode.ToLateBadTime;
                _settings.IsAccelerationEnabled = true;
            }
            
            if(GUI.Button(new Rect(120, 120,80,20), "Остановить ускорение"))
            {
                StopAcceleration();
            }
        }

        private void StopAcceleration()
        {
            if (_mode == FastTimeMode.ToBadTime)
            {
                StopAccelerationToBadTime();
                return;
            }

            StopAccelerationToLateBadTime();
        }

        private void StopAccelerationToBadTime()
        {
            _gameTimeController.OnBedtime -= StopAccelerationToBadTime;
            _settings.IsAccelerationEnabled = false;
        }
        
        private void StopAccelerationToLateBadTime()
        {
            _gameTimeController.OnLateBedtime -= StopAccelerationToLateBadTime;
            _settings.IsAccelerationEnabled = false;
        }
    }
}