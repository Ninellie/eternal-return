using EternalReturn.GameTime;
using EternalReturn.Input;
using EternalReturn.Learning;
using EternalReturn.Skills;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<InputManager>().AsSelf();
            
            builder.RegisterInstance(gameTimeSettings);
            builder.RegisterInstance(clockIndicator).Keyed("clock");
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

            builder.RegisterInstance(learningProcessSettings);
            builder.RegisterEntryPoint<LearningController>().AsSelf();
            builder.RegisterInstance(startLearningButton).Keyed("start-learning");
            builder.RegisterEntryPoint<StartLearningButtonViewController>();
            builder.RegisterEntryPoint<LearningSkillChoiceViewController>();
            builder.RegisterInstance(learningGraphic);
            builder.RegisterInstance(learningDurationBar).Keyed("learning-duration");
            builder.RegisterEntryPoint<LearningViewController>();
            
            // Skills
            builder.RegisterEntryPoint<SkillsController>().AsSelf();
            builder.RegisterEntryPoint<SkillsViewController>();
            builder.RegisterInstance(skillsPanel).Keyed("skills-panel");
            builder.RegisterInstance(draggingSkillContainer).Keyed("skill-container");
            builder.RegisterInstance(skillSocketViewPrefab);
            builder.RegisterInstance(skillViewPrefab);
        }
    }
}
