using EternalReturn.Controllers;
using TMPro;
using UnityEngine;

namespace EternalReturn.View
{
    public class IdeaButtonLabelView : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private IdeaController ideaController;
        [SerializeField] private TextMeshProUGUI text;
        
        [Header("Settings")]
        [SerializeField] private string readyText;
        [SerializeField] private string harvestableText;
        [SerializeField] private string onCooldownText;
        
        private void OnEnable()
        {
            ideaController.OnIdeaCooldownStarted += SetOnCooldown;
            ideaController.OnIdeaHarvestable += SetHarvestable;
            ideaController.OnIdeaPostHarvestCooldownExpired += SetReady;
            
            UpdateButtonLabel();    
        }

        private void OnDisable()
        {
            ideaController.OnIdeaCooldownStarted -= SetOnCooldown;
            ideaController.OnIdeaHarvestable -= SetHarvestable;
            ideaController.OnIdeaPostHarvestCooldownExpired -= SetReady;
        }

        private void UpdateButtonLabel()
        {
            if (ideaController.IsOnCooldown)
            {
                SetReady();
            }

            if (ideaController.IsHarvestable)
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