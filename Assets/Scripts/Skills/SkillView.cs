using System;
using EternalReturn.ViewStyles;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EternalReturn.Skills
{
    public class SkillView : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private TMP_Text disciple;
        [SerializeField] private TMP_Text tier;
        [SerializeField] private MaskableGraphic background;
        
        [SerializeField] private ButtonStyles _buttonStyles;
        [SerializeField] private RectTransform rectTransform;

        public RectTransform RectTransform => rectTransform;
        public SkillsViewController SkillsViewController { get; set; }
        public Skill Skill { get; private set; }

        private Action<Skill> _onSelected;

        public void BindSelection(Action<Skill> onSelected)
        {
            _onSelected = onSelected;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (_onSelected == null) return;
            _onSelected.Invoke(Skill);
        }
        
        public void OnPointerDown(PointerEventData eventData)
        {
            if (_onSelected != null) return;
            SkillsViewController.StartDrag(this, eventData.position);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            if (_onSelected != null) return;
            SkillsViewController.EndDrag();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_onSelected != null) return;
            SkillsViewController.Hover(this);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_onSelected != null) return;
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