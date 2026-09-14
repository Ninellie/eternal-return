using EternalReturn.Resources_Feature;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    public class MotivationIncreaseFromLikesController : IStartable
    {
        private const int IncreaseAmount = 1;
        
        private readonly Resource _likes;
        private readonly Resource _motivation;
        
        public MotivationIncreaseFromLikesController(ResourceProvider resourceProvider) 
        {
            _likes = resourceProvider.Likes;
            _motivation = resourceProvider.Motivation;
        }
        
        public void Start()
        {
            _likes.OnFill += IncreaseMotivation;
        }

        private void IncreaseMotivation()
        {
            _motivation.Increase(IncreaseAmount);
        }
    }
}