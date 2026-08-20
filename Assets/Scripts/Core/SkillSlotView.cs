using TMPro;
using UnityEngine;

namespace EternalReturn.Core
{
    public class SkillSlotView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private GameObject root;
        [SerializeField] private SkillSlot slot;

        public GameObject Root => root;

        private void OnEnable()
        {
            if (slot == null) return;
            slot.OnOccupied += RefreshView;
        }

        private void OnDisable()
        {
            slot.OnOccupied -= RefreshView;
        }

        public void SetSlot(SkillSlot value)
        {
            slot = value;
            
            slot.OnOccupied += RefreshView;
        }

        public void RefreshView()
        {
            if (slot.IsOccupied)
            {
                label.text = $"{slot.Skill}";
            }
            else
            {
                label.text = "Свободный слот";
            }
        }
    }
}