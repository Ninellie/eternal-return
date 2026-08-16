using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn
{
    public class IdeaButton : MonoBehaviour
    {
        [SerializeField] private Idea idea;
        [SerializeField] private SkillPanel skillPanel;
        
        [SerializeField] private Button button;
        [SerializeField] private Image filler;
        
        private void OnEnable()
        {
            skillPanel.OnSlotUnlocked += RefreshIdeaButton;
            skillPanel.OnSlotOccupied += RefreshIdeaButton;
            
            idea.OnIdeaCooldownStarted += RefreshIdeaButton;
            idea.OnIdeaHarvestable += RefreshIdeaButton;
            idea.OnIdeaHarvested += RefreshIdeaButton;
            idea.OnIdeaPostHarvestCooldownExpired += RefreshIdeaButton;
            
            RefreshIdeaButton();
        }

        private void OnDisable()
        {
            skillPanel.OnSlotUnlocked -= RefreshIdeaButton;
            skillPanel.OnSlotOccupied -= RefreshIdeaButton;
            
            idea.OnIdeaCooldownStarted -= RefreshIdeaButton;
            idea.OnIdeaHarvestable -= RefreshIdeaButton;
            idea.OnIdeaHarvested -= RefreshIdeaButton;
            idea.OnIdeaPostHarvestCooldownExpired -= RefreshIdeaButton;
        }
        
        private void RefreshIdeaButton()
        {
            if (idea.IsOnPostHarvestCooldown)
            {
                button.interactable = false;
                return;
            }
            
            if (idea.IsHarvestable)
            {
                button.interactable = true;
                return;
            }
            
            if (idea.IsOnCooldown)
            {
                button.interactable = false;
                return;
            }
            
            var hasEmptyUnlockedSlots = skillPanel.Slots.Any(s => !s.IsOccupied && !s.IsLocked);
            
            button.interactable = hasEmptyUnlockedSlots;
        }
    }
}