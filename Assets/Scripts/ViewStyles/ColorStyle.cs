using UnityEngine;

namespace EternalReturn.ViewStyles
{
    [CreateAssetMenu(menuName = "Eternal Return/Style/Color")]
    public class ColorStyle : ScriptableObject
    {
        public Color Color = new(1.0f, 1.0f, 1.0f, 1.0f);
    }
}