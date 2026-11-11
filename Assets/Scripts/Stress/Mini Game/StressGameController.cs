using UnityEngine;
using VContainer.Unity;

namespace EternalReturn.Stress
{
    public class StressGameController : IFixedTickable
    {
        private Scenario _scenario;

        private bool _isInProcess;

        public void StartGame()
        {
            _isInProcess = true;
        }

        public void FixedTick()
        {
            if (!_isInProcess) return;

            _scenario.TimeToNextSpawn -= Time.fixedDeltaTime;
            if (_scenario.TimeToNextSpawn > 0) return;
            if (!_scenario.Spawns.TryDequeue(out var current)) return;

            Spawn(current.Spawn);

            if (_scenario.Spawns.TryPeek(out var next))
            {
                _scenario.TimeToNextSpawn = next.Interval;
            }
        }

        private void Spawn(Spawn spawn)
        {
        }
    }
}
