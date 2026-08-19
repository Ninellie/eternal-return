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
        
        private void OnEnable()
        {
            if (slot == null) return;
            slot.OnChanged += RefreshView;
        }

        private void OnDisable()
        {
            slot.OnChanged -= RefreshView;
        }

        public void SetSlot(SkillSlot value)
        {
            slot = value;
            
            slot.OnChanged += RefreshView;
        }

        public void RefreshView()
        {
            root.SetActive(!slot.IsLocked);

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