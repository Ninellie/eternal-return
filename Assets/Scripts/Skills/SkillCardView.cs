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
        [SerializeField] private TMP_Text disciple;
        [SerializeField] private TMP_Text tier;
        [SerializeField] private MaskableGraphic background;
        
        [SerializeField] private ButtonStyles buttonStyles;

        private SkillChoiceViewController _controller;
        private Skill _skill;
        
        public void OnPointerClick(PointerEventData eventData)
        {
            _controller.OnCardSelected(_skill);
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            SetHighlighted();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            SetNormal();
        }

        private void SetNormal()
        {
            background.color = buttonStyles.Main.normalColor;
            disciple.color = buttonStyles.Active.normalColor;
            tier.color = buttonStyles.Active.normalColor;
        }

        private void SetHighlighted()
        {
            background.color = buttonStyles.Active.normalColor;
            disciple.color = buttonStyles.Main.normalColor;
            tier.color = buttonStyles.Main.normalColor;
        }

        public void Init(SkillChoiceViewController controller, Skill skill)
        {
            _controller = controller; 
            _skill = skill;
            
            disciple.text = skill.Discipline;
            tier.text = skill.Tier.ToRoman();
            
            SetNormal();
        }
    }
}