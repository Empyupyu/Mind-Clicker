using Main.Scripts.Views;
using UnityEngine;
using Zenject;

public class LevelViewInstaller : MonoInstaller
{
    [SerializeField] private TimerView timerView;
    [SerializeField] private BossUIView bossUIView;
    [SerializeField] private MoneyView moneyView;
    [SerializeField] private ThoughtUIView thoughtViewPrefab;
    [SerializeField] private MindView mindView;
    [SerializeField] private CharacterView characterView;
    [SerializeField] private UpgradeShopView upgradeShopView;
    [SerializeField] private CemeteryEnvironmentView cemeteryEnvironmentView;
    [SerializeField] private Light directionLight;

    public override void InstallBindings()
    {
        Container.Bind<TimerView>().FromInstance(timerView).AsSingle();
        Container.Bind<BossUIView>().FromInstance(bossUIView).AsSingle();
        Container.Bind<MoneyView>().FromInstance(moneyView).AsSingle();
        Container.Bind<CharacterView>().FromInstance(characterView).AsSingle();
        Container.Bind<ThoughtUIView>().FromInstance(thoughtViewPrefab).AsSingle();
        Container.Bind<Light>().FromInstance(directionLight).AsSingle();
        Container.Bind<MindView>().FromComponentInNewPrefab(mindView).AsSingle();
        Container.Bind<UpgradeShopView>().FromComponentInNewPrefab(upgradeShopView).AsSingle();
    }
}
