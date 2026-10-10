using EternalReturn.GameTime;
using EternalReturn.Input;
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
        
        [SerializeField] private RectTransform topicSlot1;
        [SerializeField] private RectTransform topicSlot2;
        [SerializeField] private RectTransform topicSlot3;
        [SerializeField] private RectTransform topicSlot4;
        [SerializeField] private RectTransform topicSlot5;

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
            builder.RegisterEntryPoint<CreateTopicButtonViewController>();
            builder.RegisterEntryPoint<TopicSlotsViewController>();
            
            builder.RegisterInstance(topicSlot1).Keyed("topic-slot-1");
            builder.RegisterInstance(topicSlot2).Keyed("topic-slot-2");
            builder.RegisterInstance(topicSlot3).Keyed("topic-slot-3");
            builder.RegisterInstance(topicSlot4).Keyed("topic-slot-4");
            builder.RegisterInstance(topicSlot5).Keyed("topic-slot-5");
            
        }
    }
}
