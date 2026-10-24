using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.ViewStyles
{
    [CreateAssetMenu(menuName = "Eternal Return/Button Styles")]
    public class ButtonStyles : ScriptableObject
    {
        [field: SerializeField]
        public ColorBlock Active { get; private set; }

        [field: SerializeField]
        public ColorBlock Disabled { get; private set; }

        [field: SerializeField]
        public ColorBlock Main { get; private set; }
    }
}
