using System;
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
        
        public SkillView SkillView { get; set; }
        
        private SkillSocket _skillSocket;
        
        public void SetSocket(SkillSocket socket)
        {
            _skillSocket = socket;
            
            var style = GetSocketStyle(socket.IsLocked);

            outline.FillCenter = socket.IsLocked;
            
            outline.color = style.outlineColor;
            background.color = style.backgroundColor;
            
            lockText.color = buttonStyle.Active.normalColor;
            lockText.text = !_skillSocket.IsLocked ? "" : "Lock";
        }
        
        private (Color outlineColor, Color backgroundColor) GetSocketStyle(bool isLocked)
        {
            if (isLocked) return  (buttonStyle.Disabled.normalColor,  buttonStyle.Disabled.pressedColor);
            return  (buttonStyle.Main.normalColor,  buttonStyle.Main.pressedColor);
        }
    }
}