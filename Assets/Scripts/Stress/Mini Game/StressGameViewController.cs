using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Stress.Mini_Game
{
    public class StressGameViewController : IStartable
    {
        private readonly RectTransform _panel;
        private readonly StressGameGraphic _graphic;
        private readonly StressGameConfig _config;
        private readonly StressGameController _controller;

        public StressGameViewController(
            [Key("stress-game-panel")] RectTransform panel,
            StressGameGraphic graphic,
            StressGameConfig config,
            StressGameController controller)
        {
            _panel = panel;
            _graphic = graphic;
            _config = config;
            _controller = controller;
        }

        public void Start()
        {
            _controller.OnGameStarted += OnGameStarted;
            _controller.OnGameLost += OnGameFinished;
            _controller.OnGameWon += OnGameFinished;
        }

        private void OnGameStarted()
        {
            _panel.gameObject.SetActive(true);
            _graphic.IsRendering = true;
            ApplyFieldToRect(_graphic.RectTransform);
        }

        private void OnGameFinished()
        {
            _graphic.IsRendering = false;
            _panel.gameObject.SetActive(false);
        }

        private void ApplyFieldToRect(RectTransform rect)
        {
            var field = _config.GameField;

            rect.anchorMin = new Vector2(0.5f, 0.5f);
            rect.anchorMax = new Vector2(0.5f, 0.5f);
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = field.Transform.Position;
            rect.localRotation = Quaternion.Euler(0f, 0f, field.Transform.Rotation);
            rect.sizeDelta = Vector2.Scale(field.Size, field.Transform.Scale);
        }
    }
}
