using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EternalReturn
{
    public class LikeButton : MonoBehaviour
    {
        [SerializeField] private Button button;
        
        [Inject]
        private LikeResource _resource;

        public void Like()
        {
            _resource.Increase();
        }
    }
}