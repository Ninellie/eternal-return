using TMPro;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    public class ClockViewController : IStartable, ITickable
    {
        private const string ClockTextFormat = "HH:mm";

        private readonly TextMeshProUGUI _clockIndicator;
        private readonly TextMeshProUGUI _dayIndicator;
        private readonly GameTimeController _gameTimeController;
        private readonly DayCycleController _dayCycleController;

        public ClockViewController(
            [Key("clock")] TextMeshProUGUI clockIndicator,
            [Key("day")] TextMeshProUGUI dayIndicator,
            GameTimeController gameTimeController,
            DayCycleController dayCycleController)
        {
            _clockIndicator = clockIndicator;
            _dayIndicator = dayIndicator;
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
            _clockIndicator.text = _gameTimeController.GameTime.ToString(ClockTextFormat);
            _dayIndicator.text = $"День {_gameTimeController.GetDay().ToString()}";
        }

        private void Show()
        {
            _clockIndicator.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _clockIndicator.gameObject.SetActive(false);
        }
    }
}
