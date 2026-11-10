using EternalReturn.Learning;
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
        [SerializeField] private Image icon;
        [SerializeField] private SkillsConfig skillsConfig;
        [SerializeField] private MaskableGraphic background;
        
        [SerializeField] private RectTransform rectTransform;
        
        [Header("Color styles")]
        [SerializeField] private ColorStyle backgroundColor;
        [SerializeField] private ColorStyle highlightedColor;
        [SerializeField] private ColorStyle disabledColor;
        [SerializeField] private ColorStyle iconColor;
        [SerializeField] private ColorStyle tierColor;
        
        
        public RectTransform RectTransform => rectTransform;
        public SkillsViewController SkillsViewController { get; set; }
        public Skill Skill { get; private set; }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            SkillsViewController.StartDrag(this, eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SkillsViewController.EndDrag();
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
            icon.sprite = skillsConfig.GetIconByDiscipline(skill.Discipline);

            background.color = backgroundColor.Color;
            
            icon.color = iconColor.Color;
            tier.color = tierColor.Color;
            
            Skill = skill;
            disciple.text = skill.Discipline;
            tier.text = skill.Tier.ToRoman();
        }

        public void SetNormal()
        {
            background.color = backgroundColor.Color;
        }
        
        public void SetHighlighted()
        {
            background.color = highlightedColor.Color;
        }
        
        public void SetDisabled()
        {
            background.color = disabledColor.Color;
        }
    }
}