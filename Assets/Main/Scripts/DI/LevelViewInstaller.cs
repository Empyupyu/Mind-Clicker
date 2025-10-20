using UnityEngine;
using Zenject;

public class LevelViewInstaller : MonoInstaller
{
    [SerializeField] private TimerView timerView;

    public override void InstallBindings()
    {
        Container.Bind<TimerView>().FromInstance(timerView).AsSingle();
    }
}
