using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.ViewStyles
{
    public class GraphicStyler : MonoBehaviour
    {
        [SerializeField] private ColorStyle style;
        [SerializeField] private Graphic graphic;
        
        private void OnValidate()
        {
            if (graphic == null)
            {
                graphic = GetComponent<Graphic>();
            }
            
            graphic.color = style.Color;
        }

        private void OnEnable()
        {
            if (graphic == null)
            {
                graphic = GetComponent<Graphic>();
            }
            
            graphic.color = style.Color;
        }
    }
}