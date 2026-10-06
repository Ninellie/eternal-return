using System;
using UnityEngine;

namespace EternalReturn.GameTime
{
    [CreateAssetMenu(menuName = "Eternal Return/Game Time Settings")]
    public class GameTimeSettings : ScriptableObject
    {
        public TimeOfDay NormalWakeTime;
        public TimeOfDay LateWakeTime;
        public TimeOfDay NormalBedtime;
        public TimeOfDay LateBedtime;
        public float MinutesPerRealSecond;

        [Serializable]
        public struct TimeOfDay
        {
            public int Hours;
            public int Minutes;

            public TimeSpan ToTimeSpan()
            {
                return new TimeSpan(Hours, Minutes, 0);
            }
        }
    }
}
