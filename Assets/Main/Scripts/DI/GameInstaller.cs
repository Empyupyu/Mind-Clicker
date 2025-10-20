using Main.Scripts.Views;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [SerializeField] private LoadingView loadingView;
    [SerializeField] private MindData mindData;
    [SerializeField] private PlayerDefaultSettings playerDefaultSettings;
    [SerializeField] private SoundConfig soundConfig;
    [SerializeField] private AudioPlayer audioPlayer; 

    public override void InstallBindings()
    {
        Container.Bind<DefaultPlayerDataProvider>().AsSingle().WithArguments(playerDefaultSettings);

        Container.Bind<PlayerData>().AsSingle();
        Container.Bind<PlayerDataRef>().AsSingle();
        Container.Bind<GameData>().AsSingle();
        Container.Bind<MindData>().FromScriptableObject(mindData).AsSingle();
        Container.Bind<SoundConfig>().FromScriptableObject(soundConfig).AsSingle();

        Container.Bind<IGameStateView>().FromComponentInNewPrefab(loadingView).AsSingle();

        Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();

        Container.Bind<IPlayerDataProvider>().To<YandexCloudDataProvider>().AsSingle();
        Container.Bind<SaveLoadService>().AsSingle();
        Container.Bind<AddressableAssetLoader>().AsSingle();

        BindStates();
        BindStateMachine();
    }

    private void BindStates()
    {
        Container.Bind<LoadingLevelState>().AsSingle();
        Container.Bind<LevelState>().AsSingle();
        Container.Bind<InitializeState>().AsSingle();
    }

    private void BindStateMachine()
    {
        Container.Bind<GameStateMachine>().AsSingle()
                    .OnInstantiated<GameStateMachine>((ctx, machine) =>
                    {
                        machine.Register<LoadingLevelState>(ctx.Container.Resolve<LoadingLevelState>());
                        machine.Register<LevelState>(ctx.Container.Resolve<LevelState>());
                        machine.Register<InitializeState>(ctx.Container.Resolve<InitializeState>());
                    });

        Container.Bind<GameFlowController>().FromNewComponentOnNewGameObject()
            .WithGameObjectName("GameFlowController")
            .AsSingle()
            .NonLazy();
    }
}