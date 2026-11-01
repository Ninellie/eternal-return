using EternalReturn.Learning;
using EternalReturn.Topics;
using UnityEngine;
using VContainer;

namespace EternalReturn.GameTime
{ 
    public class GameTimeDebugButtons : MonoBehaviour
    {
        private const float ButtonX = 120f;
        private const float ButtonY = 40f;
        private const float ButtonWidth = 240f;
        private const float ButtonHeight = 60f;
        private const float ButtonStep = 70f;

        private enum FastTimeMode
        {
            ToBadTime,
            ToLateBadTime
        }
        
        [Inject] private readonly GameTimeController _gameTimeController;
        [Inject] private readonly GameTimeSettings _settings;
        [Inject] private readonly ForumController _forumController;
        [Inject] private readonly LearningController _learningController;

        private FastTimeMode _mode;

        private void OnGUI()
        {
            if (GUI.Button(ButtonRect(0), "Ускорить до времени спать"))
            {
                _gameTimeController.OnBedtime += StopAccelerationToBadTime;
                _mode = FastTimeMode.ToBadTime;
                _settings.IsAccelerationEnabled = true;
            }
            
            if (GUI.Button(ButtonRect(1), "Ускорить до завершения дня"))
            {
                _gameTimeController.OnLateBedtime += StopAccelerationToLateBadTime;
                _mode = FastTimeMode.ToLateBadTime;
                _settings.IsAccelerationEnabled = true;
            }
            
            if (GUI.Button(ButtonRect(2), "Остановить ускорение"))
            {
                StopAcceleration();
            }

            if (GUI.Button(ButtonRect(3), "Abort тем"))
            {
                _forumController.Abort();
            }

            if (GUI.Button(ButtonRect(4), "Abort обучения"))
            {
                _learningController.Abort();
            }
        }

        private static Rect ButtonRect(int index)
        {
            return new Rect(ButtonX, ButtonY + ButtonStep * index, ButtonWidth, ButtonHeight);
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