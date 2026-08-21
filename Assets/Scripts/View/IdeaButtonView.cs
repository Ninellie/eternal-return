using System.Linq;
using EternalReturn.Controllers;
using EternalReturn.Core;
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
            skillPanelController.OnSlotCreated += RefreshIdeaControllerButton;
            skillPanelController.OnSlotOccupied += RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested += RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired += RefreshIdeaControllerButton;
            
            RefreshIdeaControllerButton();
        }

        private void OnDisable()
        {
            skillPanelController.OnSlotCreated -= RefreshIdeaControllerButton;
            skillPanelController.OnSlotOccupied -= RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested -= RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired -= RefreshIdeaControllerButton;
        }

        private void RefreshIdeaControllerButton(SkillSlot slot)
        {
            RefreshIdeaControllerButton();
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
            
            var hasEmptySlots = skillPanelController.Slots.Any(s => !s.IsOccupied);
            
            button.interactable = hasEmptySlots;
        }
    }
}