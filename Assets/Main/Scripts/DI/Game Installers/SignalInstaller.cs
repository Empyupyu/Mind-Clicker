using Zenject;

public class SignalInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);

        Container.DeclareSignal<GameLoadedSignal>();
        Container.DeclareSignal<SoundEffectSignal>();
        Container.DeclareSignal<PrestigeSignal>();
    }
}
