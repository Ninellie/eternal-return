using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace EternalReturn
{
    public class LikeIndicator : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private Image image;

        [Inject]
        private LikeResource _resource;

        private void OnEnable()
        {
            UpdateIndicator(_resource.Amount);
            _resource.OnChange += UpdateIndicator;
        }

        private void OnDisable()
        {
            _resource.OnChange -= UpdateIndicator;
        }

        private void UpdateIndicator(int amount)
        {
            text.text = amount.ToString();
            
            var percent = _resource.Amount / (float)_resource.MaxAmount;
            
            image.rectTransform.anchorMax = new Vector2(percent, 1);
        }
    }
}