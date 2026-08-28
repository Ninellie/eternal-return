using System.Linq;
using EternalReturn.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.Ideas
{
    public class IdeaButtonView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private IdeaController ideaController;
        [SerializeField] private SkillsController skillsController;
        [SerializeField] private Button button;
        
        private void OnEnable()
        {
            skillsController.OnSocketCreated += RefreshIdeaControllerButton;
            skillsController.OnSocketOccupied += RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable += RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested += RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired += RefreshIdeaControllerButton;
            
            RefreshIdeaControllerButton();
        }

        private void OnDisable()
        {
            skillsController.OnSocketCreated -= RefreshIdeaControllerButton;
            skillsController.OnSocketOccupied -= RefreshIdeaControllerButton;
            
            ideaController.OnIdeaCooldownStarted -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvestable -= RefreshIdeaControllerButton;
            ideaController.OnIdeaHarvested -= RefreshIdeaControllerButton;
            ideaController.OnIdeaPostHarvestCooldownExpired -= RefreshIdeaControllerButton;
        }

        private void RefreshIdeaControllerButton(SkillSocket socket)
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
            
            var hasEmptySlots = skillsController.Sockets.Any(s => !s.IsOccupied);
            
            button.interactable = hasEmptySlots;
        }
    }
}