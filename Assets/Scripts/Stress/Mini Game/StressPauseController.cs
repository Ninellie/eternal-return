using EternalReturn.GameTime;
using EternalReturn.Input;
using EternalReturn.Learning;
using EternalReturn.Resources_Feature;
using EternalReturn.Topics;
using VContainer.Unity;

namespace EternalReturn.Stress.Mini_Game
{
    public class StressPauseController : IStartable
    {
        private readonly Resource _stress;
        private readonly StressGameController _stressGameController;
        private readonly GameTimeController _gameTimeController;
        private readonly LearningController _learningController;
        private readonly ForumController _forumController;
        private readonly InputManager _inputManager;

        private bool _isPaused;

        public StressPauseController(
            ResourceProvider resourceProvider,
            StressGameController stressGameController,
            GameTimeController gameTimeController,
            LearningController learningController,
            ForumController forumController,
            InputManager inputManager)
        {
            _stress = resourceProvider.Stress;
            _stressGameController = stressGameController;
            _gameTimeController = gameTimeController;
            _learningController = learningController;
            _forumController = forumController;
            _inputManager = inputManager;
        }

        public void Start()
        {
            _stress.OnFill += PauseAll;
            _stressGameController.OnGameWon += ResumeAll;
            _stressGameController.OnGameLost += ResumeAll;
        }

        private void PauseAll()
        {
            if (_isPaused) return;

            _isPaused = true;
            _gameTimeController.Pause();
            _learningController.Pause();
            _forumController.Pause();
            _inputManager.SwitchToStressMap();
        }

        private void ResumeAll()
        {
            if (!_isPaused) return;

            _isPaused = false;
            _gameTimeController.Resume();
            _learningController.Resume();
            _forumController.Resume();
            _inputManager.SwitchToPlayerMap();
        }
    }
}
