using Cysharp.Threading.Tasks;
using DG.Tweening;
using Main.Scripts.Views;
using UnityEngine;
using Zenject;

public class GameBootstrap : MonoBehaviour
{
    private IModuleInitializer moduleInitializer;
    private ILevelLoader levelLoader;
    private ILoadingView loadingView;

    [Inject]
    public void Construct(
        IModuleInitializer moduleInitializer,
        ILevelLoader levelLoader,
        ILoadingView loadingView)
    {
        this.moduleInitializer = moduleInitializer;
        this.levelLoader = levelLoader;
        this.loadingView = loadingView;
    }

    private void Start()
    {
        RunAsync().Forget();
    }

    private async UniTask RunAsync()
    {
        loadingView.Show();

        await moduleInitializer.InitializeModulesAsync();
        await levelLoader.LoadLevelAsync("Game");

        loadingView.Hide().Forget();
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        DOTween.Clear();
    }
}
