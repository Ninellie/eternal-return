using EternalReturn.GameTime;
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

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(gameTimeSettings);
            builder.RegisterInstance(clockIndicator);
            builder.RegisterInstance(endDayButton).Keyed("end-day");
            builder.RegisterInstance(endDayButtonLabel).Keyed("end-day");
            builder.RegisterInstance(startDayButton).Keyed("start-day");

            builder.RegisterEntryPoint<GameTimeController>().AsSelf();
            builder.RegisterEntryPoint<DayCycleController>().AsSelf();
            builder.RegisterEntryPoint<ClockViewController>();
            builder.RegisterEntryPoint<EndDayButtonViewController>();
            builder.RegisterEntryPoint<StartDayButtonViewController>();
        }
    }
}
