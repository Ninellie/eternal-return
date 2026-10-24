using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EternalReturn.Skills
{
    public class SkillView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text disciple;
        [SerializeField] private TMP_Text tier;
        [SerializeField] private MaskableGraphic background;
        
        [SerializeField] private ButtonStyles _buttonStyles;
        
        public SkillsViewController SkillsViewController { get; set; }
        public Skill Skill { get; private set; }
        public SkillSocketView Socket { get; private set; }

        public void OnPointerDown(PointerEventData eventData)
        {
            SkillsViewController.Press(this, eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SkillsViewController.Release();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            SkillsViewController.Hover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SkillsViewController.EndHover();
        }

        public void SetSkill(Skill skill)
        {
            background.color = _buttonStyles.Active.normalColor;
            
            disciple.color = _buttonStyles.Main.normalColor;
            tier.color = _buttonStyles.Main.normalColor;
            
            Skill = skill;
            disciple.text = skill.Discipline;
            tier.text = skill.Tier.ToRoman();
        }

        public void SetSkillSocketView(SkillSocketView skillSocketView)
        {
            Socket = skillSocketView;
        }

        public void SetNormal()
        {
            background.color = _buttonStyles.Active.normalColor;
        }
        
        public void SetHighlighted()
        {
            background.color = _buttonStyles.Active.highlightedColor;
        }
        
        public void SetDisabled()
        {
            background.color = _buttonStyles.Active.disabledColor;
        }
    }
}