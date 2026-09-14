using UnityEngine.UI;
using VContainer.Unity;

namespace EternalReturn.Ideas
{
    public class IdeaCooldownIndicatorViewController : IStartable, ITickable
    {
        private readonly IdeaController _ideaController;
        private readonly Image _filler;

        private readonly IdeaIndicatorColors _colors = new();

        public IdeaCooldownIndicatorViewController(IdeaController ideaController, IdeaView ideaView)
        {
            _ideaController = ideaController;
            _filler = ideaView.Filler;
        }

        public void Start()
        {
            _ideaController.OnIdeaCooldownStarted += SetOnCooldown;
            _ideaController.OnIdeaHarvestable += SetHarvestable;
            
            _ideaController.OnIdeaHarvested += SetOnPostHarvestCooldown;
            _ideaController.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateFillAmount();
        }

        public void Tick()
        {
            if (!_ideaController.IsOnPostHarvestCooldown && !_ideaController.IsOnCooldown) return;
            UpdateFillAmount();
        }
        
        private void UpdateFillAmount()
        {
            if (_ideaController.IsOnPostHarvestCooldown)
            {
                _filler.fillAmount = _ideaController.PostHarvestCooldown / _ideaController.BasePostHarvestCooldown;
                return;
            }

            if (_ideaController.IsOnCooldown)
            {
                _filler.fillAmount = 1 - _ideaController.Cooldown / _ideaController.BaseCooldown;
                return;
            }
            
            _filler.fillAmount = 0;
        }

        private void SetOnPostHarvestCooldown() => _filler.color = _colors.OnPostHarvestCooldownColor;
        
        private void SetReady() => _filler.fillAmount = 0;

        private void SetHarvestable() => _filler.color = _colors.HarvestableColor;

        private void SetOnCooldown() => _filler.color = _colors.OnCooldownColor;
    }
}