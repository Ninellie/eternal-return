using System;

namespace EternalReturn.Resources_Feature
{
    [Serializable]
    public class ResourceProvider
    {
        public Resource Likes;
        public Resource Overheat;
        public Resource Intel;
        public Resource Motivation;
        public Resource Stress;
        public Resource Burnout;

        public ResourceProvider(ResourceConfigs resourceConfig)
        {
            Likes = new Resource(resourceConfig.Likes);
            Overheat = new Resource(resourceConfig.Overheat);
            Intel = new Resource(resourceConfig.Intel);
            Motivation = new Resource(resourceConfig.Motivation);
            Stress = new Resource(resourceConfig.Stress);
            Burnout = new Resource(resourceConfig.Burnout);
        }
    }
}