using System;
using System.Linq;
using EternalReturn.Burnout;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using UnityEngine;
using UnityEngine.UI;
using VContainer.Unity;

namespace EternalReturn.Ideas
{
    public class IdeaController : IStartable, IFixedTickable, ISlowableByBurnout
    {
        private const float Slowdown = 0.05f;

        public float BaseCooldown { get; private set; }
        public float BasePostHarvestCooldown { get; private set; }
        
        public bool IsOnCooldown { get; private set; }
        public float Cooldown { get; private set; }
        public bool IsOnPostHarvestCooldown { get; private set; }
        public float PostHarvestCooldown { get; private set; }

        public bool IsHarvestable { get; private set; }

        public bool IsSlowed { get; private set; }

        public event Action OnIdeaCooldownStarted;
        public event Action OnIdeaPostHarvestCooldownExpired;
        public event Action OnIdeaHarvestable;
        public event Action OnIdeaHarvested;
        
        private readonly IdeasRepository _ideaRepository;
        private readonly SkillsController _skillsController;
        private readonly Button _ideaButton;
        private readonly Resource _burnout;

        private Idea _idea;

        public IdeaController(
            IdeasRepository ideaRepository,
            SkillsController skillsController,
            IdeaView ideaView,
            ResourceProvider resourceProvider)
        {
            _ideaRepository = ideaRepository;
            _skillsController = skillsController;
            _ideaButton = ideaView.Button;
            _burnout = resourceProvider.Burnout;
        }

        public void Start()
        {
            _ideaButton.onClick.AddListener(GetIdea);
            _burnout.OnFill += Slow;
            _burnout.OnDecrease += _ => Unslow();
        }

        public void FixedTick()
        {
            var deltaTime = Time.fixedDeltaTime;

            if (IsSlowed)
            {
                deltaTime *= Slowdown;
            }

            if (IsOnPostHarvestCooldown)
            {
                PostHarvestCooldown -= deltaTime;
                
                if (PostHarvestCooldown > 0) return;
                
                PostHarvestCooldown = 0;
                
                IsOnPostHarvestCooldown = false;
                
                OnIdeaPostHarvestCooldownExpired?.Invoke();
            }
            
            if (!IsOnCooldown) return;
            
            Cooldown -= deltaTime;
            
            if (Cooldown > 0) return;
            
            Cooldown = 0;
            
            IsOnCooldown = false;
            IsHarvestable = true;
            
            OnIdeaHarvestable?.Invoke();
        }

        public void Slow()
        {
            IsSlowed = true;
        }

        public void Unslow()
        {
            IsSlowed = false;
        }

        private void GetIdea()
        {
            if (IsOnCooldown) return;
            if (IsOnPostHarvestCooldown) return;
            
            if (IsHarvestable)
            {
                var skill = new Skill(_idea.SkillConfig);
                
                _skillsController.InsertSkill(skill);
                
                _idea = null;
                IsHarvestable = false;
                IsOnPostHarvestCooldown = true;
                
                PostHarvestCooldown = BasePostHarvestCooldown;
                OnIdeaHarvested?.Invoke();
                return;
            }

            var hasEmptyUnlockedSlots = _skillsController.Sockets.Any(s => !s.IsOccupied);
            
            if (!hasEmptyUnlockedSlots) return;
            
            _idea = _ideaRepository.GetRandomIdea();
            
            BaseCooldown = _idea.HarvestCooldown;
            BasePostHarvestCooldown = _idea.PostHarvestCooldown;
            Cooldown = BaseCooldown;
            IsOnCooldown = true;
            
            OnIdeaCooldownStarted?.Invoke();
        }
    }
}