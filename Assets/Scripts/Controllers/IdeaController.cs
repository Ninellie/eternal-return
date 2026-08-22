using System;
using System.Linq;
using EternalReturn.Core;
using UnityEngine;

namespace EternalReturn.Controllers
{
    public class IdeaController : MonoBehaviour
    {
        [Header("Dependencies")]        
        [SerializeField] private IdeasRepository ideaRepository;

        [SerializeField] private SkillPanelController skillPanelController;
        
        [Header("Settings")]
        [SerializeField] private float baseCooldown;
        [SerializeField] private float cooldown;
        [SerializeField] private bool isOnCooldown;

        [SerializeField] private float basePostHarvestCooldown;
        [SerializeField] private bool isOnPostHarvestCooldown;
        [SerializeField] private float postHarvestCooldown;
        
        [SerializeField] private bool isHarvestable;
        
        [Header("Inner")]
        [SerializeField] private Idea idea;
        
        public bool IsOnCooldown => isOnCooldown;
        public float BaseCooldown => baseCooldown;
        public float Cooldown => cooldown;
        
        public bool IsOnPostHarvestCooldown => isOnPostHarvestCooldown;
        public float BasePostHarvestCooldown => basePostHarvestCooldown;
        public float PostHarvestCooldown => postHarvestCooldown;
        
        public bool IsHarvestable => isHarvestable;

        public event Action OnIdeaCooldownStarted;
        public event Action OnIdeaPostHarvestCooldownExpired;
        public event Action OnIdeaHarvestable;
        public event Action OnIdeaHarvested;
        
        public void GetIdea()
        {
            if (isOnCooldown) return;
            if (isOnPostHarvestCooldown) return;
            
            if (isHarvestable)
            {
                var skill = new Skill(idea.SkillConfig);
                
                skillPanelController.AddSkill(skill);
                
                idea = null;
                isHarvestable = false;
                isOnPostHarvestCooldown = true;
                
                postHarvestCooldown = basePostHarvestCooldown;
                OnIdeaHarvested?.Invoke();
                return;
            }

            var hasEmptyUnlockedSlots = skillPanelController.Slots.Any(s => !s.IsOccupied);
            
            if (!hasEmptyUnlockedSlots) return;
            
            idea = ideaRepository.GetRandomIdea();
            
            baseCooldown = idea.HarvestCooldown;
            cooldown = baseCooldown;
            isOnCooldown = true;
            
            OnIdeaCooldownStarted?.Invoke();
        }

        private void FixedUpdate()
        {
            if (isOnPostHarvestCooldown)
            {
                postHarvestCooldown -= Time.fixedDeltaTime;
                
                if (postHarvestCooldown > 0) return;
                
                postHarvestCooldown = 0;
                
                isOnPostHarvestCooldown = false;
                
                OnIdeaPostHarvestCooldownExpired?.Invoke();
            }
            
            if (!isOnCooldown) return;
            
            cooldown -= Time.fixedDeltaTime;
            
            if (cooldown > 0) return;
            
            cooldown = 0;
            
            isOnCooldown = false;
            isHarvestable = true;
            
            OnIdeaHarvestable?.Invoke();
        }
    }
}