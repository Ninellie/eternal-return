using System.Collections.Generic;
using EternalReturn.Ideas;
using EternalReturn.Likes;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using EternalReturn.Stress;
using EternalReturn.Topics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace EternalReturn
{
    public class GameLifetimeScope : LifetimeScope
    {
        // Resources
        [SerializeField] private ResourceConfigs resourceConfigs;
        [SerializeField] private ResourceProvider resourceProvider;
        [SerializeField] private TextMeshProUGUI intelIndicator;
        [SerializeField] private TextMeshProUGUI motivationIndicator;
        [SerializeField] private TextMeshProUGUI stressIndicator;
        
        // Ideas
        [SerializeField] private IdeasRepository ideasRepository;
        [SerializeField] private IdeaView ideaView;
        
        // Likes
        [SerializeField] private Image likesIndicator;
        [SerializeField] private Image overheatIndicator;
        [SerializeField] private Button likeButton;
        
        // Skills
        [SerializeField] private SkillSocketView skillSocketPrefab;
        [SerializeField] private RectTransform skillSocketContentContainer;
        [SerializeField] private Button buySkillSocketButton;
        [SerializeField] private TextMeshProUGUI buySkillSocketButtonLabel;
        
        // Topics
        [SerializeField] private TopicsRepository topicsRepository;
        [SerializeField] private TopicSocketView topicSocketPrefab;
        [SerializeField] private RectTransform topicSocketContentContainer;
        [SerializeField] private Button buyTopicSocketButton;
        [SerializeField] private TextMeshProUGUI buyTopicSocketButtonLabel;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Resources
            resourceProvider = new ResourceProvider(resourceConfigs);
            builder.RegisterInstance(resourceProvider);

            // Indicators
            builder.RegisterEntryPoint<ResourceIndicatorViewController>();
            
            var textIndicators = new Dictionary<Resource, TextMeshProUGUI>
            {
                { resourceProvider.Intel, intelIndicator },
                { resourceProvider.Motivation, motivationIndicator },
                { resourceProvider.Stress, stressIndicator }
            };

            var imageIndicators = new Dictionary<Resource, Image>()
            {
                { resourceProvider.Likes, likesIndicator },
                { resourceProvider.Overheat, overheatIndicator },
            };
            
            builder.RegisterInstance(textIndicators);
            builder.RegisterInstance(imageIndicators);
            
            
            // Likes
            builder.RegisterEntryPoint<LikesBlockController>();
            builder.RegisterEntryPoint<MotivationIncreaseFromLikesController>();
            builder.RegisterEntryPoint<OverheatController>();
            builder.RegisterEntryPoint<LikesViewController>();
            builder.RegisterInstance(likeButton).Keyed("likes");
            
            
            // Stress
            builder.RegisterEntryPoint<StressController>();
            
            // Idea
            builder.RegisterEntryPoint<IdeaController>().AsSelf();
            builder.RegisterEntryPoint<IdeaButtonViewController>();
            builder.RegisterEntryPoint<IdeaButtonLabelViewController>();
            builder.RegisterEntryPoint<IdeaCooldownIndicatorViewController>();
            builder.RegisterInstance(ideasRepository);
            builder.RegisterInstance(ideaView);
            
            // Skills
            builder.RegisterEntryPoint<SkillsController>().AsSelf();
            builder.RegisterEntryPoint<SkillsViewController>();
            builder.RegisterInstance(buySkillSocketButton).Keyed("skills");
            builder.RegisterInstance(buySkillSocketButtonLabel).Keyed("skills"); // todo объединить
            builder.RegisterInstance(skillSocketContentContainer).Keyed("skills");
            builder.RegisterInstance(skillSocketPrefab);
            
            // Topics
            builder.RegisterEntryPoint<TopicsController>().AsSelf();
            builder.RegisterEntryPoint<TopicsViewController>();
            builder.RegisterInstance(topicsRepository);
            builder.RegisterInstance(topicSocketPrefab);
            builder.RegisterInstance(topicSocketContentContainer).Keyed("topics"); // todo объединить
            builder.RegisterInstance(buyTopicSocketButton).Keyed("topics");
            builder.RegisterInstance(buyTopicSocketButtonLabel).Keyed("topics");
        }
    }
}