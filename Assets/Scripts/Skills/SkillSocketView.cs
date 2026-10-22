using EternalReturn.ViewStyles;
using UnityEngine;

namespace EternalReturn.Skills
{
    public class SkillSocketView : MonoBehaviour
    {
        [SerializeField] private H_SDFUIOutline background;
        [SerializeField] private ButtonStyles buttonStyle;
        
        public SkillView SkillView { get; set; }

        private SkillSocket _skillSocket;
        
        public void SetSocket(SkillSocket socket)
        {
            _skillSocket = socket;

            background.FillCenter = _skillSocket.IsLocked;
            
            if (_skillSocket.IsLocked)
            {
                background.color = buttonStyle.Active.disabledColor;
            }
            else
            {
                background.color = buttonStyle.Active.normalColor;
            }
        }
    }
}