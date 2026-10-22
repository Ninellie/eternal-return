using UnityEngine;
using UnityEngine.EventSystems;

namespace EternalReturn.Skills
{
    public class SkillView : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        public SkillsViewController SkillsViewController { get; set; }

        public Skill Skill { get; private set; }

        public void SetSkill(Skill skill)
        {
            Skill = skill;
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