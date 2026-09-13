using EternalReturn.Resources_Feature;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    public class MotivationIncreaseFromLikesController : IStartable
    {
        private const int IncreaseAmount = 1;
        
        private readonly Resource _likes;
        private readonly Resource _motivation;
        
        public MotivationIncreaseFromLikesController([Key("likes")] Resource likes, [Key("motivation")] Resource overheat)
        {
            _likes = likes;
            _motivation = overheat;
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