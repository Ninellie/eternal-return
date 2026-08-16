using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn
{
    public class IdeaCooldownIndicator : MonoBehaviour
    {
        [SerializeField] private Idea idea;
        
        [SerializeField] private Image filler;
        [SerializeField] private Color harvestableColor;
        [SerializeField] private Color onCooldownColor;
        [SerializeField] private Color onPostHarvestCooldownColor;
        
        private void OnEnable()
        {
            idea.OnIdeaCooldownStarted += SetOnCooldown;
            idea.OnIdeaHarvestable += SetHarvestable;
            
            idea.OnIdeaHarvested += SetOnPostHarvestCooldown;
            idea.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateFillAmount();
        }

        private void OnDisable()
        {
            idea.OnIdeaCooldownStarted -= SetOnCooldown;
            idea.OnIdeaHarvestable -= SetHarvestable;
            
            idea.OnIdeaHarvested -= SetOnPostHarvestCooldown;
            idea.OnIdeaPostHarvestCooldownExpired -= SetReady;
        }

        private void Update()
        {
            if (!idea.IsOnPostHarvestCooldown && !idea.IsOnCooldown) return;
            UpdateFillAmount();
        }

        private void UpdateFillAmount()
        {
            if (idea.IsOnPostHarvestCooldown)
            {
                filler.fillAmount = idea.PostHarvestCooldown / idea.BasePostHarvestCooldown;
                return;
            }

            if (idea.IsOnCooldown)
            {
                filler.fillAmount = 1 - idea.Cooldown / idea.BaseCooldown;
                return;
            }
            
            filler.fillAmount = 0;
        }

        private void SetOnPostHarvestCooldown()
        {
            filler.color = onPostHarvestCooldownColor;
        }

        private void SetReady()
        {
            filler.fillAmount = 0;
        }

        private void SetHarvestable()
        {
            filler.color = harvestableColor;
        }

        private void SetOnCooldown()
        {
            filler.color = onCooldownColor;
        }
    }
}