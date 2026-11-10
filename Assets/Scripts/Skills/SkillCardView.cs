using EternalReturn.Learning;
using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EternalReturn.Skills
{
    public class SkillCardView : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private Image icon;
        [SerializeField] private TMP_Text tier;
        [SerializeField] private MaskableGraphic background;
        [SerializeField] private SkillsConfig skillConfig;
        
        [Header("Color styles")]
        [SerializeField] private ColorStyle backgroundColor;
        [SerializeField] private ColorStyle iconColor;
        [SerializeField] private ColorStyle tierColor;
        
        [SerializeField] private ColorStyle highlightedBackgroundColor;
        [SerializeField] private ColorStyle highlightedIconColor;
        [SerializeField] private ColorStyle highlightedTierColor;

        private SkillChoiceViewController _controller;
        private Skill _skill;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _controller.OnCardSelected(_skill);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            _controller.OnCardPointerEnter(_skill);
            SetHighlighted();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _controller.OnCardPointerExit();
            SetNormal();
        }

        private void SetNormal()
        {
            background.color = backgroundColor.Color;
            icon.color = iconColor.Color;
            tier.color = tierColor.Color;
        }

        private void SetHighlighted()
        {
            background.color = highlightedBackgroundColor.Color;
            icon.color = highlightedIconColor.Color;
            tier.color = highlightedTierColor.Color;
        }

        public void Init(SkillChoiceViewController controller, Skill skill)
        {
            _controller = controller; 
            _skill = skill;
            
            icon.sprite = skillConfig.GetIconByDiscipline(skill.Discipline);
            
            tier.text = skill.Tier.ToRoman();
            
            SetNormal();
        }
    }
}