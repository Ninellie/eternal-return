using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;

namespace EternalReturn.Skills
{
    public class SkillSocketView : MonoBehaviour
    {
        [SerializeField] private H_SDFUIOutline outline;
        [SerializeField] private H_SDFUIOutline background;
        [SerializeField] private ButtonStyles buttonStyle;
        [SerializeField] private TMP_Text lockText;
        
        public SkillSocket SkillSocket { get; private set; }
        public SkillView SkillView { get; set; }
        
        
        public void SetSocket(SkillSocket socket)
        {
            SkillSocket = socket;
            
            var style = GetSocketStyle(socket.IsLocked);

            outline.FillCenter = socket.IsLocked;
            
            outline.color = style.outlineColor;
            background.color = style.backgroundColor;
            
            lockText.color = buttonStyle.Active.normalColor;
            lockText.text = !SkillSocket.IsLocked ? "" : "Lock";
        }
        
        private (Color outlineColor, Color backgroundColor) GetSocketStyle(bool isLocked)
        {
            if (isLocked) return  (buttonStyle.Disabled.normalColor,  buttonStyle.Disabled.pressedColor);
            return  (buttonStyle.Main.normalColor,  buttonStyle.Main.pressedColor);
        }

        public void SetHighlighted()
        {
            background.color = buttonStyle.Main.highlightedColor;
        }
        
        public void SetNormal()
        {
            background.color = buttonStyle.Main.pressedColor;
        }
    }
}