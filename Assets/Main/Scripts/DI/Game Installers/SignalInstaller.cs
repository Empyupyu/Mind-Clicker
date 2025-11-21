using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<LevelLoadedSignal>();
        Container.DeclareSignal<SoundEffectSignal>();
        Container.DeclareSignal<MuteSoundsSignal>();
        Container.DeclareSignal<AdvertisementRewardButtonClickedSignal>();
        Container.DeclareSignal<RewardButtonInitializedSignal>();
        Container.DeclareSignal<RewardCooldownUpdatedSignal>();
    }
}
