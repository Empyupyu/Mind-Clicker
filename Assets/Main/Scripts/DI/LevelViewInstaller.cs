using UnityEngine;
using Zenject;

public class LevelViewInstaller : MonoInstaller
{
    [SerializeField] private TimerView timerView;
    [SerializeField] private BossView bossView;
    [SerializeField] private MoneyView moneyView;

    public override void InstallBindings()
    {
        Container.Bind<TimerView>().FromInstance(timerView).AsSingle();
        Container.Bind<BossView>().FromInstance(bossView).AsSingle();
        Container.Bind<MoneyView>().FromInstance(moneyView).AsSingle();
    }
}
