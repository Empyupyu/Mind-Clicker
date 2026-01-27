using UnityEngine;
using Zenject;

public class GameConfigInstaller : MonoInstaller
{
    [SerializeField] private MindData mindData;
    [SerializeField] private AdvertisementData advertisementData;
    [SerializeField] private RewardSettings rewardSettings;
    [SerializeField] private PlayerDefaultSettings playerDefaultSettings;
    [SerializeField] private SoundConfig soundConfig;
    [SerializeField] private AudioConfig audioConfig;
    [SerializeField] private ProjectSettingsConfig projectSettingsConfig;
    [SerializeField] private RewardCooldownsConfig rewardCooldownsConfig;
    [SerializeField] private ModulePrioritiesConfig modulePrioritiesConfig;

    public override void InstallBindings()
    {
        Container.Bind<MindData>().FromScriptableObject(mindData).AsSingle();
        Container.Bind<AdvertisementData>().FromScriptableObject(advertisementData).AsSingle();
        Container.Bind<RewardSettings>().FromScriptableObject(rewardSettings).AsSingle();
        Container.Bind<PlayerDefaultSettings>().FromInstance(playerDefaultSettings).AsSingle();
        Container.Bind<SoundConfig>().FromScriptableObject(soundConfig).AsSingle();
        Container.Bind<AudioConfig>().FromScriptableObject(audioConfig).AsSingle();
        Container.Bind<ProjectSettingsConfig>().FromScriptableObject(projectSettingsConfig).AsSingle();
        Container.Bind<DefaultPlayerDataProvider>().AsSingle().WithArguments(playerDefaultSettings);
        Container.Bind<RewardCooldownsConfig>().FromInstance(rewardCooldownsConfig).AsSingle();
        Container.Bind<ModulePrioritiesConfig>().FromInstance(modulePrioritiesConfig).AsSingle();
    }
}