using EternalReturn.Controllers;
using UnityEngine;
using UnityEngine.UI;

namespace EternalReturn.View
{
    public class IdeaCooldownIndicatorView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private IdeaController ideaController;
        [SerializeField] private Image filler;
        
        [Header("Settings")]
        [SerializeField] private Color harvestableColor;
        [SerializeField] private Color onCooldownColor;
        [SerializeField] private Color onPostHarvestCooldownColor;
        
        private void OnEnable()
        {
            ideaController.OnIdeaCooldownStarted += SetOnCooldown;
            ideaController.OnIdeaHarvestable += SetHarvestable;
            
            ideaController.OnIdeaHarvested += SetOnPostHarvestCooldown;
            ideaController.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateFillAmount();
        }

        private void OnDisable()
        {
            ideaController.OnIdeaCooldownStarted -= SetOnCooldown;
            ideaController.OnIdeaHarvestable -= SetHarvestable;
            
            ideaController.OnIdeaHarvested -= SetOnPostHarvestCooldown;
            ideaController.OnIdeaPostHarvestCooldownExpired -= SetReady;
        }

        private void Update()
        {
            if (!ideaController.IsOnPostHarvestCooldown && !ideaController.IsOnCooldown) return;
            UpdateFillAmount();
        }

        private void UpdateFillAmount()
        {
            if (ideaController.IsOnPostHarvestCooldown)
            {
                filler.fillAmount = ideaController.PostHarvestCooldown / ideaController.BasePostHarvestCooldown;
                return;
            }

            if (ideaController.IsOnCooldown)
            {
                filler.fillAmount = 1 - ideaController.Cooldown / ideaController.BaseCooldown;
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