using System.Collections.Generic;

namespace EternalReturn.Stress.Mini_Game
{
    public class Scenario
    {
        public Queue<(float Interval, Spawn Spawn)> Spawns { get; }
        public float TimeToNextSpawn { get; set; }
        public float TimeLeft { get; set; }

        public Scenario(Queue<(float Interval, Spawn Spawn)> spawns, float duration)
        {
            Spawns = spawns;
            TimeToNextSpawn = spawns.TryPeek(out var next) ? next.Interval : 0f;
            TimeLeft = duration;
        }
    }
}
