using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelConfigInstaller : MonoInstaller
{
    [SerializeField, Expandable] private List<UpgradeConfig> upgradeConfigs;
    [SerializeField, Expandable] private NegativeThoughtConfig thoughtConfigs;
    [SerializeField, Expandable] private BossEnvironmentManifest bossEnvironmentManifest;
    [SerializeField, Expandable] private CharacterConfig characterConfig;
    [SerializeField, Expandable] private LightConfig originLightConfig;
    [SerializeField, Expandable] private SphereArcConfig sphereArcConfig;
    [SerializeField, Expandable] private BossFightData bossFightData;
    [SerializeField, Expandable] private List<ThoughtSpawnPointData> thoughtSpawnPointDatas;

    public override void InstallBindings()
    {
        Container.Bind<CharacterConfig>().FromInstance(characterConfig).AsSingle();
        Container.Bind<BossFightData>().FromScriptableObject(bossFightData).AsSingle();
        Container.Bind<LightConfig>().FromInstance(originLightConfig).AsSingle();
        Container.Bind<SphereArcConfig>().FromInstance(sphereArcConfig).AsSingle();
        Container.Bind<BossEnvironmentManifest>().FromInstance(bossEnvironmentManifest).AsSingle();
        Container.Bind<NegativeThoughtConfig>().FromInstance(thoughtConfigs).AsSingle();
        Container.Bind<List<UpgradeConfig>>().FromInstance(upgradeConfigs).AsSingle();
        Container.Bind<List<ThoughtSpawnPointData>>().FromInstance(thoughtSpawnPointDatas).AsSingle();
    }
}
