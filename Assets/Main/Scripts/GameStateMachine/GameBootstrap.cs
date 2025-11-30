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
    private SaveLoadService saveLoadService;
    private PlayerDataRef playerDataRef;

    private const string LevelKey = "Game";

    [Inject]
    public void Construct(
        IModuleInitializer moduleInitializer,
        ILevelLoader levelLoader,
        ILoadingView loadingView,
        SaveLoadService saveLoadService,
        PlayerDataRef playerDataRef)
    {
        this.moduleInitializer = moduleInitializer;
        this.levelLoader = levelLoader;
        this.loadingView = loadingView;
        this.saveLoadService = saveLoadService;
        this.playerDataRef = playerDataRef;
    }

    private void Start()
    {
        EnterPoint().Forget();
    }

    private async UniTask EnterPoint()
    {
        loadingView.Show();

        await moduleInitializer.InitializeModulesAsync();
        await levelLoader.LoadLevelAsync(LevelKey);

        loadingView.Hide().Forget();
    }

    private void OnDisable()
    {
        DOTween.KillAll();
        DOTween.Clear();
    }

    //TODO
    private void OnApplicationQuit()
    {
        saveLoadService.Save(playerDataRef.Value);
    }

    private void OnApplicationPause(bool pauseStatus)
    {
        if (pauseStatus)
            saveLoadService.Save(playerDataRef.Value);
    }
}
