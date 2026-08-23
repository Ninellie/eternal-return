using EternalReturn.Resources_Feature;
using UnityEngine;

namespace EternalReturn.Likes
{
    public class LikesIncreaseController : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private ResourceRepository resourceRepository;
        
        [Header("Settings")]
        [SerializeField] private string likeResourceName;
        [SerializeField] private int increaseAmount;

        private Resource _resource;

        private void OnEnable()
        {
            _resource = resourceRepository.GetByName(likeResourceName);
        }

        private void OnDisable()
        {
            _resource = null;
        }

        /// <summary>
        /// Метод вызывает кнопка из инспектора
        /// </summary>
        public void Increase()
        {
            _resource.Increase(increaseAmount);
        }
    }
}