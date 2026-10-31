using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.GameTime
{
    public class EndDayButtonViewController : IStartable, ITickable
    {
        private const float PulseAmplitude = 0.05f;
        private const float PulseSpeed = 4f;

        private readonly Button _button;
        private readonly Graphic _graphic;
        private readonly Transform _label;
        private readonly DayCycleController _dayCycleController;
        private readonly GameTimeController _gameTimeController;

        private bool _isPulsing;

        public EndDayButtonViewController(
            [Key("end-day")] Button endDayButton,
            [Key("end-day")] TextMeshProUGUI endDayButtonLabel,
            DayCycleController dayCycleController,
            GameTimeController gameTimeController)
        {
            _button = endDayButton;
            _graphic = endDayButton.targetGraphic;
            _label = endDayButtonLabel.transform;
            _dayCycleController = dayCycleController;
            _gameTimeController = gameTimeController;
        }

        public void Start()
        {
            _dayCycleController.OnDayEnded += Hide;
            _gameTimeController.OnBedtime += Show;
            _button.onClick.AddListener(_dayCycleController.EndDay);
            Hide();
        }

        public void Tick()
        {
            if (!_isPulsing) return;

            var wave = Mathf.Sin(Time.time * PulseSpeed);
            var scale = 1f + wave * PulseAmplitude;
            ApplyScale(scale);
            _graphic.color = Color.Lerp(Color.white, Color.red, Mathf.Clamp01(wave));
        }

        private void Show()
        {
            _button.gameObject.SetActive(true);
            _isPulsing = true;
        }

        private void Hide()
        {
            StopPulsing();
            _button.gameObject.SetActive(false);
        }

        private void StopPulsing()
        {
            _isPulsing = false;
            ApplyScale(1f);
            _graphic.color = Color.white;
        }

        private void ApplyScale(float scale)
        {
            _button.transform.localScale = new Vector3(scale, scale, 1f);
            var labelScale = 1f / scale;
            _label.localScale = new Vector3(labelScale, labelScale, 1f);
        }
    }
}
