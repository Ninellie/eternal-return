using System.Collections.Generic;

namespace EternalReturn.Stress
{
    public class Scenario
    {
        public Queue<(float Interval, Spawn Spawn)> Spawns { get; }
        public float TimeToNextSpawn { get; set; }

        public Scenario(Queue<(float Interval, Spawn Spawn)> spawns)
        {
            Spawns = spawns;
            TimeToNextSpawn = spawns.TryPeek(out var next) ? next.Interval : 0f;
        }
    }
}
