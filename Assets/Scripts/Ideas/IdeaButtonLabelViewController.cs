using TMPro;
using VContainer.Unity;

namespace EternalReturn.Ideas
{
    public class IdeaButtonLabelViewController : IStartable
    {
        private const string ReadyText = "Думать";
        private const string HarvestableText = "Идея";
        private const string OnCooldownText = "Хмм...";
        
        private readonly TextMeshProUGUI _label;
        private readonly IdeaController _ideaController;

        public IdeaButtonLabelViewController(IdeaController ideaController, IdeaView ideaView)
        {
            _label = ideaView.Label;
            _ideaController = ideaController;
        }

        public void Start()
        {
            _ideaController.OnIdeaCooldownStarted += SetOnCooldown;
            _ideaController.OnIdeaHarvestable += SetHarvestable;
            _ideaController.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateButtonLabel();
        }

        private void UpdateButtonLabel()
        {
            if (_ideaController.IsOnCooldown)
            {
                SetOnCooldown();
                return;
            }

            if (_ideaController.IsHarvestable)
            {
                SetHarvestable();
                return;
            }

            SetReady();
        }

        private void SetReady()
        {
            _label.text = ReadyText;
        }

        private void SetHarvestable()
        {
            _label.text = HarvestableText;
        }

        private void SetOnCooldown()
        {
            _label.text = OnCooldownText;
        }
    }
}