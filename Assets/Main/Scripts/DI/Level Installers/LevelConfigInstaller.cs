using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelConfigInstaller : MonoInstaller
{
    [SerializeField] private List<UpgradeConfig> upgradeConfigs;
    [SerializeField] private NegativeThoughtConfig thoughtConfigs;
    [SerializeField] private CharacterConfig characterConfig;
    [SerializeField] private List<SphereArcSpawner> sphereArcSpawners;

    public override void InstallBindings()
    {
        Container.Bind<CharacterConfig>().FromInstance(characterConfig).AsSingle();
        Container.Bind<NegativeThoughtConfig>().FromInstance(thoughtConfigs).AsSingle();
        Container.Bind<List<UpgradeConfig>>().FromInstance(upgradeConfigs).AsSingle();
        Container.Bind<List<SphereArcSpawner>>().FromInstance(sphereArcSpawners).AsSingle();
    }
}
