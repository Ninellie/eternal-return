using System.Linq;
using EternalReturn.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.View
{
    public class IdeaButtonView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private IdeaController ideaController;
        [SerializeField] private SkillPanelController skillPanelController;
        [SerializeField] private Button button;
        
        private void OnEnable()
        {
            skillPanelController.OnSlotUnlocked += RefreshIdeaControllerButton;
            skillPanelController.OnSlotOccupied += RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested += RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired += RefreshIdeaControllerButton;
            
            RefreshIdeaControllerButton();
        }

        private void OnDisable()
        {
            skillPanelController.OnSlotUnlocked -= RefreshIdeaControllerButton;
            skillPanelController.OnSlotOccupied -= RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested -= RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired -= RefreshIdeaControllerButton;
        }
        
        private void RefreshIdeaControllerButton()
        {
            if (ideaController.IsOnPostHarvestCooldown)
            {
                button.interactable = false;
                return;
            }
            
            if (ideaController.IsHarvestable)
            {
                button.interactable = true;
                return;
            }
            
            if (ideaController.IsOnCooldown)
            {
                button.interactable = false;
                return;
            }
            
            var hasEmptyUnlockedSlots = skillPanelController.Slots.
                Any(s => !s.IsOccupied && !s.IsLocked);
            
            button.interactable = hasEmptyUnlockedSlots;
        }
    }
}