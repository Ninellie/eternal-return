using EternalReturn.Resources_Feature;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Likes
{
    public class LikesViewController : IStartable
    {
        private const int IncreaseAmount = 1;
        
        private readonly Resource _likes;
        private readonly Button _likeButton;

        public LikesViewController([Key("likes")] Button likeButton, ResourceProvider resourceProvider)
        {
            _likeButton = likeButton;
            _likes = resourceProvider.Likes;
        }

        public void Start()
        {
            _likeButton.onClick.AddListener(Increase);
            
            _likes.OnBlocked += () => _likeButton.interactable = false;
            _likes.OnUnblocked += () => _likeButton.interactable = true;
        }
        
        private void Increase()
        {
            _likes.Increase(IncreaseAmount);
        }
    }
}