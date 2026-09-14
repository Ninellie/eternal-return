using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using VContainer.Unity;

namespace EternalReturn.Resources_Feature
{
    public class ResourceIndicatorViewController : IStartable
    {
        private readonly Dictionary<Resource, Image> _imageIndicators;
        private readonly Dictionary<Resource, TextMeshProUGUI> _textIndicators;

        public ResourceIndicatorViewController(
            Dictionary<Resource, Image> imageIndicators,
            Dictionary<Resource, TextMeshProUGUI> textIndicators)
        {
            _textIndicators = textIndicators;
            _imageIndicators = imageIndicators;
        }

        public void Start()
        {
            foreach (var (resource, image) in (_imageIndicators))
            {
                resource.OnChange += _ => { image.fillAmount = resource.Amount / (float)resource.MaxAmount; };
            }
            
            foreach (var (resource, text) in (_textIndicators))
            {
                resource.OnChange += _ => { text.text = resource.Amount.ToString(); };
            }
        }
    }
}