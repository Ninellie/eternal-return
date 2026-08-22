using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Core
{
    public class SkillSlotView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Image filler;
        
        [SerializeField] private SkillSlot slot;

        private void OnEnable()
        {
            if (slot == null) return;
            slot.OnOccupied += RefreshView;
        }

        private void OnDisable()
        {
            slot.OnOccupied -= RefreshView;
        }

        private void Update()
        {
            if (!slot.IsOccupied) return;
            
            var percent = 1 - slot.Skill.Cooldown / slot.Skill.BaseCooldown;
            
            filler.rectTransform.anchorMax = new Vector2(percent, 1);
        }

        public void SetSlot(SkillSlot value)
        {
            slot = value;
            SetEmpty();
            slot.OnOccupied += RefreshView;
        }

        public void RefreshView()
        {
            if (slot.IsOccupied)
            {
                label.text = $"{slot.Skill.Name}";
            }
            else
            {
                label.text = "Свободный слот";
            }
        }

        private void SetEmpty()
        {
            filler.rectTransform.anchorMax = new Vector2(0, 1);
        }
    }
}