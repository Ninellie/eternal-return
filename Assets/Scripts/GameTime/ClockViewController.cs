using System.Globalization;
using TMPro;
using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    public class ClockViewController : IStartable, ITickable
    {
        private const string ClockTextFormat = "dd.MM.yyyy\nHH:mm";

        private readonly TextMeshProUGUI _clockIndicator;
        private readonly GameObject _clock;
        private readonly GameTimeController _gameTimeController;
        private readonly DayCycleController _dayCycleController;

        public ClockViewController(
            TextMeshProUGUI clockIndicator,
            GameTimeController gameTimeController,
            DayCycleController dayCycleController)
        {
            _clockIndicator = clockIndicator;
            _clock = clockIndicator.transform.parent.gameObject;
            _gameTimeController = gameTimeController;
            _dayCycleController = dayCycleController;
        }

        public void Start()
        {
            _dayCycleController.OnDayEnded += Hide;
            _dayCycleController.OnDayStarted += Show;
            Show();
        }

        public void Tick()
        {
            _clockIndicator.text = _gameTimeController.GameTime.ToString(ClockTextFormat, CultureInfo.InvariantCulture);
        }

        private void Show()
        {
            _clock.SetActive(true);
        }

        private void Hide()
        {
            _clock.SetActive(false);
        }
    }
}
