using System.Linq;
using EternalReturn.Skills;
using UnityEngine.UI;
using VContainer.Unity;

namespace EternalReturn.Ideas
{
    public class IdeaButtonViewController : IStartable
    {
        private readonly IdeaController _ideaController;
        private readonly SkillsController _skillsController;
        private readonly Button _button;

        public IdeaButtonViewController(
            IdeaController ideaController,
            SkillsController skillsController,
            IdeaView ideaView)
        {
            _ideaController = ideaController;
            _skillsController = skillsController;
            _button = ideaView.Button;
        }

        public void Start()
        {
            _skillsController.OnSocketCreated += RefreshIdeaControllerButton;
            _skillsController.OnSocketOccupied += RefreshIdeaControllerButton;
            
            _ideaController.OnIdeaCooldownStarted += RefreshIdeaControllerButton;
            _ideaController.OnIdeaHarvestable += RefreshIdeaControllerButton;
            _ideaController.OnIdeaHarvested += RefreshIdeaControllerButton;
            _ideaController.OnIdeaPostHarvestCooldownExpired += RefreshIdeaControllerButton;
            
            RefreshIdeaControllerButton();
        }

        private void RefreshIdeaControllerButton(SkillSocket socket)
        {
            RefreshIdeaControllerButton();
        }

        private void RefreshIdeaControllerButton()
        {
            if (_ideaController.IsOnPostHarvestCooldown)
            {
                _button.interactable = false;
                return;
            }
            
            if (_ideaController.IsHarvestable)
            {
                _button.interactable = true;
                return;
            }
            
            if (_ideaController.IsOnCooldown)
            {
                _button.interactable = false;
                return;
            }
            
            var hasEmptySlots = _skillsController.Sockets.Any(s => !s.IsOccupied);
            
            _button.interactable = hasEmptySlots;
        }
    }
}