using TMPro;
using UnityEngine;

namespace EternalReturn
{
    public class IdeaButtonLabel : MonoBehaviour
    {
        [SerializeField] private Idea idea;
        
        [SerializeField] private string readyText;
        [SerializeField] private string harvestableText;
        [SerializeField] private string onCooldownText;
        
        [SerializeField] private TextMeshProUGUI text;
        
        private void OnEnable()
        {
            idea.OnIdeaCooldownStarted += SetOnCooldown;
            idea.OnIdeaHarvestable += SetHarvestable;
            idea.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateButtonLabel();    
        }

        private void OnDisable()
        {
            idea.OnIdeaCooldownStarted -= SetOnCooldown;
            idea.OnIdeaHarvestable -= SetHarvestable;
            idea.OnIdeaPostHarvestCooldownExpired -= SetReady;
        }

        private void UpdateButtonLabel()
        {
            if (idea.IsOnCooldown)
            {
                SetReady();
            }

            if (idea.IsHarvestable)
            {
                SetHarvestable();
            }
            
            SetReady();
        }
        
        private void SetReady()
        {
            text.text = readyText;
        }

        private void SetHarvestable()
        {
            text.text = harvestableText;
        }

        private void SetOnCooldown()
        {
            text.text = onCooldownText;
        }
    }
}