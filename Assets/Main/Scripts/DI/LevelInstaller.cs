using Main.Scripts.Views;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private ThoughtUIView thoughtViewPrefab;
    [SerializeField] private MindView mindView;
    [SerializeField] private CharacterView characterView;
    [SerializeField] private List<SphereArcSpawner> sphereArcSpawners;
    [SerializeField] private UpgradeShopView upgradeShopViews;

    [SerializeField] private List<UpgradeConfig> upgradeConfigs;
    [SerializeField] private NegativeThoughtConfig thoughtConfigs;
    [SerializeField] private CharacterConfig characterConfig;
    [SerializeField] private CemeteryEnvironmentView cemeteryEnvironmentView;

    public override void InstallBindings()
    {
        BindUpgrades();
        BindThoughtFactory();

        Container.BindInterfacesAndSelfTo<ThoughtSpawner>().AsSingle().OnInstantiated<ThoughtSpawner>((ctx, spawner) =>
        {
            spawner.SetFactory(ctx.Container.Resolve<ThoughtFactory>());
        });

        Container.Bind<MindView>().FromInstance(mindView).AsSingle();
        Container.Bind<CharacterView>().FromInstance(characterView).AsSingle();
        Container.Bind<UpgradeShopView>().FromInstance(upgradeShopViews).AsSingle();
        Container.Bind<ThoughtUIView>().FromInstance(thoughtViewPrefab).AsSingle();

        Container.Bind<CharacterConfig>().FromInstance(characterConfig).AsSingle();
        Container.Bind<NegativeThoughtConfig>().FromInstance(thoughtConfigs).AsSingle();

        Container.Bind<List<SphereArcSpawner>>().FromInstance(sphereArcSpawners).AsSingle();

        Container.Bind<Mind>().AsSingle();
        Container.Bind<UpgradeMaterialAnimation>().AsSingle();
        Container.BindInterfacesAndSelfTo<DealDamage>().AsSingle();
        Container.BindInterfacesAndSelfTo<DamageFeedbackService>().AsSingle();
        Container.BindInterfacesAndSelfTo<MindController>().AsSingle();
        Container.Bind<Timer>().AsSingle();

        BindWallet();
    }

    private void BindWallet()
    {
        Container.Bind<MoneyWallet>().AsSingle();
        Container.BindInterfacesAndSelfTo<MoneyController>().AsSingle();
        Container.BindInterfacesAndSelfTo<ThoughtRewardHandler>().AsSingle();
    }

    private void BindThoughtFactory()
    {
        Container.Bind<CemeteryEnvironmentView>().FromInstance(cemeteryEnvironmentView).AsSingle();
        
        Container.BindInterfacesAndSelfTo<BossFightPrepare>().AsTransient();

        Container.Bind<IThoughtLogic>().To<Tier1EnemyThoughtLogic>().AsTransient().WithArguments(ThoughtType.Tier1Enemy);
        Container.Bind<IThoughtLogic>().To<BossBubbleThoughtLogic>().AsTransient().WithArguments(ThoughtType.Boss1);
        Container.Bind<IThoughtLogic>().To<BossCemeteryThoughtLogic>().AsTransient().WithArguments(ThoughtType.BossCemetery);

        Container.BindInterfacesAndSelfTo<ThoughtFactory>().AsSingle();
    }

    private void BindUpgrades()
    {
        Container.Bind<IUpgradeEffect>().To<AddClickDamageEffect>().AsSingle().WithArguments(upgradeConfigs.Find(config => config.Type.ToString().Equals(typeof(AddClickDamageEffect).Name)));

        Container.Bind<IUpgradeEffect>().To<AddDamagePerSecondTiear1Effect>().AsSingle().WithArguments(upgradeConfigs.Find(config => config.Type.ToString().Equals(typeof(AddDamagePerSecondTiear1Effect).Name)));

        Container.BindInterfacesAndSelfTo<Upgrade>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSaveService>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeController>().AsSingle();
        Container.BindInterfacesAndSelfTo<UpgradeSoundFeedbackService>().AsSingle();
    }
}
