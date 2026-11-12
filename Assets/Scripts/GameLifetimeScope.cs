using EternalReturn.GameTime;
using EternalReturn.Input;
using EternalReturn.Learning;
using EternalReturn.Resources_Feature;
using EternalReturn.Skills;
using EternalReturn.Stress;
using EternalReturn.Stress.Collision_System;
using EternalReturn.Stress.Mini_Game;
using EternalReturn.Stress.Movement_System;
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
        [SerializeField] private GameTimeSettings gameTimeSettings;
        [SerializeField] private TextMeshProUGUI clockIndicator;
        [SerializeField] private TextMeshProUGUI dayIndicator;
        [SerializeField] private Button endDayButton;
        [SerializeField] private TextMeshProUGUI endDayButtonLabel;
        [SerializeField] private Button startDayButton;
        
        [SerializeField] private Button createTopicButton;
        [SerializeField] private TopicSettings topicSettings;
        
        [SerializeField] private RectTransform topicSlotsRoot;
        [SerializeField] private TopicView topicViewPrefab;

        [SerializeField] private Button startLearningButton;
        [SerializeField] private LearningGraphic learningGraphic;
        [SerializeField] private RectTransform learningDurationBar;
        [SerializeField] private LearningProcessSettings learningProcessSettings;
        
        [SerializeField] private Transform skillsPanel;
        [SerializeField] private Transform draggingSkillContainer;
        [SerializeField] private SkillSocketView skillSocketViewPrefab;
        [SerializeField] private SkillView skillViewPrefab;
        [SerializeField] private SkillCardView skillCardViewPrefab;
        [SerializeField] private SkillsConfig skillsConfig;
        [SerializeField] private SkillChoicePanel skillChoicePanel;
        
        [Header("Stress")] 
        [SerializeField] private RectTransform stressBar;
        [SerializeField] private StressConfig stressConfig;
        [SerializeField] private StressGameConfig stressGameConfig;
        
        [Header("Resources")] 
        [SerializeField] private ResourceConfigs resourceConfigs;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputManager>().AsSelf();
            
            builder.RegisterInstance(gameTimeSettings);
            builder.RegisterInstance(clockIndicator).Keyed("clock");
            builder.RegisterInstance(dayIndicator).Keyed("day");
            builder.RegisterInstance(endDayButton).Keyed("end-day");
            builder.RegisterInstance(endDayButtonLabel).Keyed("end-day");
            builder.RegisterInstance(startDayButton).Keyed("start-day");

            builder.RegisterEntryPoint<GameTimeController>().AsSelf();
            builder.RegisterEntryPoint<DayCycleController>().AsSelf();
            builder.RegisterEntryPoint<ClockViewController>();
            builder.RegisterEntryPoint<EndDayButtonViewController>();
            builder.RegisterEntryPoint<StartDayButtonViewController>();
            
            builder.RegisterInstance(topicSettings);
            builder.RegisterInstance(createTopicButton).Keyed("create-topic");
            builder.RegisterEntryPoint<ForumController>().AsSelf();
            
            builder.RegisterEntryPoint<ForumViewController>();
            builder.RegisterEntryPoint<CreateTopicButtonViewController>();
            builder.RegisterInstance(topicSlotsRoot).Keyed("topics-root");
            builder.RegisterInstance(topicViewPrefab);

            // Learning
            builder.RegisterEntryPoint<LearningController>().AsSelf();
            builder.RegisterEntryPoint<LearningViewController>();
            builder.RegisterInstance(learningGraphic);
            builder.RegisterInstance(learningDurationBar).Keyed("learning-duration");
            builder.RegisterInstance(learningProcessSettings);
            builder.RegisterInstance(skillChoicePanel);
            
            // Learning start
            builder.RegisterEntryPoint<StartLearningButtonViewController>();
            builder.RegisterInstance(startLearningButton).Keyed("start-learning");

            // Choosing skills
            builder.RegisterEntryPoint<SkillChoiceViewController>();
            builder.RegisterInstance(skillCardViewPrefab);
            builder.RegisterInstance(skillsConfig);
            
            // Skills
            builder.RegisterEntryPoint<SkillsController>().AsSelf();
            builder.RegisterEntryPoint<SkillsViewController>();
            builder.RegisterInstance(skillsPanel).Keyed("skills-panel");
            builder.RegisterInstance(draggingSkillContainer).Keyed("skill-container");
            builder.RegisterInstance(skillSocketViewPrefab);
            builder.RegisterInstance(skillViewPrefab);
            
            // stress
            builder.RegisterEntryPoint<StressGainController>();
            builder.RegisterEntryPoint<BurnoutGainController>();
            builder.RegisterEntryPoint<StressBarViewController>();
            builder.RegisterInstance(stressBar).Keyed("stress-bar");
            builder.RegisterInstance(stressConfig);
            builder.RegisterInstance(stressGameConfig);
            
            builder.RegisterEntryPoint<CollisionSystem>().AsSelf();
            builder.RegisterEntryPoint<MovementSystem>().AsSelf();
            builder.RegisterEntryPoint<StressGameController>().AsSelf();
            
            // resources
            var resourceProvider = new ResourceProvider(resourceConfigs);
            builder.RegisterInstance(resourceProvider);
        }
    }
}
