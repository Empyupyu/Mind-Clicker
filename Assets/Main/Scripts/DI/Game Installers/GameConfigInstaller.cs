using UnityEngine;
using Zenject;

public class GameConfigInstaller : MonoInstaller
{
    [SerializeField] private MindData mindData;
    [SerializeField] private PlayerDefaultSettings playerDefaultSettings;
    [SerializeField] private SoundConfig soundConfig;
    [SerializeField] private ProjectSettingsConfig projectSettingsConfig;

    public override void InstallBindings()
    {
        Container.Bind<MindData>().FromScriptableObject(mindData).AsSingle();
        Container.Bind<PlayerDefaultSettings>().FromInstance(playerDefaultSettings).AsSingle();
        Container.Bind<SoundConfig>().FromScriptableObject(soundConfig).AsSingle();
        Container.Bind<ProjectSettingsConfig>().FromScriptableObject(projectSettingsConfig).AsSingle();

        Container.Bind<DefaultPlayerDataProvider>().AsSingle().WithArguments(playerDefaultSettings);
    }
}
