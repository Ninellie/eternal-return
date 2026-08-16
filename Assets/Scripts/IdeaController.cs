using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn
{
    public class IdeaController : MonoBehaviour
    {
        [SerializeField] private float getIdeaCooldown;
        [SerializeField] private Button getIdeaButton;
        [SerializeField] private SkillPanel skillPanel;
        
        private void OnEnable()
        {
            skillPanel.OnSlotUnlocked += RefreshIdeaButton;
            skillPanel.OnSlotOccupied += RefreshIdeaButton;
        }

        private void OnDisable()
        {
            skillPanel.OnSlotUnlocked -= RefreshIdeaButton;
            skillPanel.OnSlotOccupied -= RefreshIdeaButton;
        }

        private void RefreshIdeaButton()
        {
            var hasEmptyUnlockedSlots = skillPanel.Slots.Any(s => s.IsEmpty && !s.IsLocked);
            
            getIdeaButton.interactable = hasEmptyUnlockedSlots;
        }
    }
}