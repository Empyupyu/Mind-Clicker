using Main.Scripts.Views;
using UnityEngine;
using Zenject;

public class GameViewInstaller : MonoInstaller
{
    [SerializeField] private LoadingView loadingView;
    [SerializeField] private AudioPlayer audioPlayer;

    public override void InstallBindings()
    {
        Container.Bind<ILoadingView>().FromComponentInNewPrefab(loadingView).AsSingle();
        Container.Bind<AudioPlayer>().FromInstance(audioPlayer).AsSingle();
    }
}
