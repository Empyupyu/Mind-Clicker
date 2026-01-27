using Main.Scripts.Views;
using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{
    [SerializeField] private LoadingView loadingView;
    [SerializeField] private ShopView shopView;
    [SerializeField] private UIWindowsView uIWindowsView;
    [SerializeField] private AudioPlayer audioPlayer;

    public override void InstallBindings()
    {
        Container.Bind<ILoadingView>().FromComponentInNewPrefab(loadingView).AsSingle();
        Container.Bind<ShopView>().FromInstance(shopView).AsSingle();
        Container.Bind<UIWindowsView>().FromInstance(uIWindowsView).AsSingle();
        Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
    }
}
