using System.Collections.Generic;
using EternalReturn.Ideas;
using EternalReturn.Likes;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using EternalReturn.Stress;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn
{
    public class GameLifetimeScope : LifetimeScope
    {
        [SerializeField] private ResourceConfigs resourceConfigs;
        
        [SerializeField] private IdeasRepository ideasRepository;
        [SerializeField] private IdeaView ideaView;
        
        [SerializeField] private Button likeButton;
        
        [SerializeField] private Image likesIndicator;
        [SerializeField] private Image overheatIndicator;
        
        [SerializeField] private TextMeshProUGUI intelIndicator;
        [SerializeField] private TextMeshProUGUI motivationIndicator;
        [SerializeField] private TextMeshProUGUI stressIndicator;
        
        [SerializeField] private Button buySkillSocketButton;
        [SerializeField] private TextMeshProUGUI buySkillSocketButtonLabel;

        [SerializeField] private RectTransform skillSocketContentContainer;
        
        [SerializeField] private SkillSocketView viewSocketPrefab;
        
        [SerializeField] private ResourceProvider resourceProvider;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Resources
            resourceProvider = new ResourceProvider(resourceConfigs);

            builder.RegisterInstance(resourceProvider);
            
            // Indicators
            var textIndicators = new Dictionary<Resource, TextMeshProUGUI>
            {
                { resourceProvider.Intel, intelIndicator },
                { resourceProvider.Intel, motivationIndicator },
                { resourceProvider.Stress, stressIndicator }
            };

            var imageIndicators = new Dictionary<Resource, Image>()
            {
                { resourceProvider.Likes, likesIndicator },
                { resourceProvider.Overheat, overheatIndicator },
            };

            builder.RegisterInstance(textIndicators);
            builder.RegisterInstance(imageIndicators);
            builder.RegisterEntryPoint<ResourceIndicatorViewController>();
            
            // Likes
            builder.RegisterInstance(likeButton).Keyed("likes");
            builder.RegisterEntryPoint<LikesBlockController>();
            builder.RegisterEntryPoint<MotivationIncreaseFromLikesController>();
            builder.RegisterEntryPoint<OverheatController>();
            builder.RegisterEntryPoint<LikesViewController>();
            
            
            // Stress
            builder.RegisterEntryPoint<StressController>();
            
            // Idea
            builder.RegisterInstance(ideaView);
            builder.RegisterInstance(ideasRepository);
            builder.RegisterEntryPoint<IdeaController>().AsSelf();
            builder.RegisterEntryPoint<IdeaButtonViewController>();
            builder.RegisterEntryPoint<IdeaButtonLabelViewController>();
            builder.RegisterEntryPoint<IdeaCooldownIndicatorViewController>();
            
            // Skills
            builder.RegisterInstance(buySkillSocketButton).Keyed("skills");
            builder.RegisterInstance(buySkillSocketButtonLabel).Keyed("skills");
            builder.RegisterInstance(skillSocketContentContainer);
            builder.RegisterInstance(viewSocketPrefab);
            builder.RegisterEntryPoint<SkillsController>().AsSelf();
            builder.RegisterEntryPoint<SkillsViewController>();
        }
    }
}