using System.Collections.Generic;
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
        [SerializeField] private SkillsController skillsController;
        
        [SerializeField] private Button likeButton;
        
        [SerializeField] private Image likesIndicator;
        [SerializeField] private Image overheatIndicator;
        
        [SerializeField] private TextMeshProUGUI intelIndicator;
        [SerializeField] private TextMeshProUGUI motivationIndicator;
        [SerializeField] private TextMeshProUGUI stressIndicator;

        protected override void Configure(IContainerBuilder builder)
        {
            // Resources
            var likes = new Resource(resourceConfigs.Likes);
            var overheat = new Resource(resourceConfigs.Overheat);
            var intel = new Resource(resourceConfigs.Intel);
            var motivation = new Resource(resourceConfigs.Motivation);
            var stress = new Resource(resourceConfigs.Stress);

            builder.RegisterInstance(likes).Keyed("likes");
            builder.RegisterInstance(overheat).Keyed("overheat");
            builder.RegisterInstance(intel).Keyed("intel");
            builder.RegisterInstance(motivation).Keyed("motivation");
            builder.RegisterInstance(stress).Keyed("stress");
            
            // Indicators
            var textIndicators = new Dictionary<Resource, TextMeshProUGUI>
            {
                { intel, intelIndicator },
                { motivation, motivationIndicator },
                { stress, stressIndicator }
            };

            var imageIndicators = new Dictionary<Resource, Image>()
            {
                { likes, likesIndicator },
                { overheat, overheatIndicator },
            };

            builder.RegisterInstance(textIndicators);
            builder.RegisterInstance(imageIndicators);
            builder.RegisterEntryPoint<ResourceIndicatorViewController>();
            
            // Likes
            builder.RegisterEntryPoint<LikesBlockController>();
            builder.RegisterEntryPoint<LikesViewController>();
            builder.RegisterEntryPoint<MotivationIncreaseFromLikesController>();
            builder.RegisterEntryPoint<OverheatController>();
            builder.RegisterInstance(likeButton).Keyed("like_button");
            
            // Stress
            builder.RegisterEntryPoint<StressController>();
            builder.RegisterInstance(skillsController);
        }
    }
}