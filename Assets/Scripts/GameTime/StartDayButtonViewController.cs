using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    public class StartDayButtonViewController : IStartable
    {
        private readonly Button _button;
        private readonly DayCycleController _dayCycleController;

        public StartDayButtonViewController(
            [Key("start-day")] Button startDayButton,
            DayCycleController dayCycleController)
        {
            _button = startDayButton;
            _dayCycleController = dayCycleController;
        }

        public void Start()
        {
            _dayCycleController.OnDayEnded += Show;
            _dayCycleController.OnDayStarted += Hide;
            _button.onClick.AddListener(_dayCycleController.StartDay);
            Hide();
        }

        private void Show()
        {
            _button.gameObject.SetActive(true);
        }

        private void Hide()
        {
            _button.gameObject.SetActive(false);
        }
    }
}
