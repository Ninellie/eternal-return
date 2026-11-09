using UnityEngine;

namespace EternalReturn.Stats
{
    [CreateAssetMenu(menuName = "Eternal Return/stat-config")]
    public class StatConfigs : ScriptableObject
    {
        [SerializeField] private StatConfig[] statConfigs;

        public StatConfig[] GetStatConfigs()
        {
            var configs = new StatConfig[statConfigs.Length];
            statConfigs.CopyTo(configs, 0);
            return configs;
        }
    }
}