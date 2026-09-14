using TMPro;
using VContainer;
using VContainer.Unity;

namespace EternalReturn.Ideas
{
    public class IdeaButtonLabelViewController : IStartable
    {
        private const string ReadyText = "Думать";
        private const string HarvestableText = "Идея";
        private const string OnCooldownText = "Хмм...";
        
        private readonly TextMeshProUGUI _text;
        private readonly IdeaController _ideaController;

        public IdeaButtonLabelViewController(IdeaController ideaController, [Key("idea")]TextMeshProUGUI text)
        {
            _text = text;
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
                SetReady();
            }

            if (_ideaController.IsHarvestable)
            {
                SetHarvestable();
            }
            
            SetReady();
        }

        private void SetReady()
        {
            _text.text = ReadyText;
        }

        private void SetHarvestable()
        {
            _text.text = HarvestableText;
        }

        private void SetOnCooldown()
        {
            _text.text = OnCooldownText;
        }
    }
}