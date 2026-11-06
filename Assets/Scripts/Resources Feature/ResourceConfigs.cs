using UnityEngine;

namespace EternalReturn.Resources_Feature
{
    [CreateAssetMenu(menuName = "Eternal Return/resource-configs")]
    public class ResourceConfigs : ScriptableObject
    {
        public ResourceConfig Intel;
        public ResourceConfig Motivation;
        public ResourceConfig Likes;
        public ResourceConfig Overheat;
        public ResourceConfig Stress;
        public ResourceConfig Burnout;
    }
}