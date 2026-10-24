using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EternalReturn.Skills
{
    public class SkillView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TMP_Text disciple;
        [SerializeField] private TMP_Text tier;
        [SerializeField] private MaskableGraphic background;
        
        [SerializeField] private ButtonStyles _buttonStyles;
        
        public SkillsViewController SkillsViewController { get; set; }
        public Skill Skill { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            SkillsViewController.Press(this, eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SkillsViewController.Release();
        }

        public void SetSkill(Skill skill)
        {
            background.color = _buttonStyles.Active.normalColor;
            
            disciple.color = _buttonStyles.Main.normalColor;
            tier.color = _buttonStyles.Main.normalColor;
            
            Skill = skill;
            disciple.text = skill.Disciple;
            tier.text = skill.Tier.ToRoman();
        }
        
        public void SetHighlighted()
        {
            // outline.
        }
    }
}