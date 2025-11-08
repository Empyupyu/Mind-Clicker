using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelConfigInstaller : MonoInstaller
{
    [SerializeField] private List<UpgradeConfig> upgradeConfigs;
    [SerializeField] private NegativeThoughtConfig thoughtConfigs;
    [SerializeField] private BossEnvironmentManifest bossEnvironmentManifest;
    [SerializeField] private CharacterConfig characterConfig;
    [SerializeField] private LightConfig originLightConfig;
    [SerializeField] private List<ThoughtSpawnPointData> thoughtSpawnPointDatas;

    public override void InstallBindings()
    {
        Container.Bind<CharacterConfig>().FromInstance(characterConfig).AsSingle();
        Container.Bind<LightConfig>().FromInstance(originLightConfig).AsSingle();
        Container.Bind<BossEnvironmentManifest>().FromInstance(bossEnvironmentManifest).AsSingle();
        Container.Bind<NegativeThoughtConfig>().FromInstance(thoughtConfigs).AsSingle();
        Container.Bind<List<UpgradeConfig>>().FromInstance(upgradeConfigs).AsSingle();
        Container.Bind<List<ThoughtSpawnPointData>>().FromInstance(thoughtSpawnPointDatas).AsSingle();
    }
}
