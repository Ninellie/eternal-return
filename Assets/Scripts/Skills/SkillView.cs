using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace EternalReturn.Skills
{
    public class SkillView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private TMP_Text disciple;
        [SerializeField] private TMP_Text tier;
        
        public SkillsViewController SkillsViewController { get; set; }
        public Skill Skill { get; private set; }

        public void SetSkill(Skill skill)
        {
            Skill = skill;
            disciple.text = skill.Disciple;
            tier.text = skill.Tier.ToString();
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            SkillsViewController.StartDrag(this);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            SkillsViewController.StopDrag();
        }
    }
}