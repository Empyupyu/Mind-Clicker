using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class BossEnvironmentController : IBossEnvironmentController
{
    protected Material originalSkybox;
    protected GameObject instance;
    protected IBossEnvironmentView bossEnvironmentView;

    protected readonly BossEnvironmentManifest bossEnvironmentManifest;
    protected readonly BossEnvironmentRegistry bossEnvironmentRegistry;
    protected readonly LightService lightService;
    protected readonly AudioPlayer audioPlayer;

    private string addressableKey;

    private readonly LevelMusicFlow levelMusicFlow;
    private readonly AddressableAssetLoader addressableAssetLoader;
    private readonly DiContainer diContainer;

    public BossEnvironmentController(
        LightService lightService, 
        AudioPlayer audioPlayer,
        LevelMusicFlow levelMusicFlow,
        BossEnvironmentManifest bossEnvironmentManifest,
        BossEnvironmentRegistry bossEnvironmentRegistry,
        AddressableAssetLoader addressableAssetLoader,
        DiContainer diContainer)
    {
        this.lightService = lightService;
        this.audioPlayer = audioPlayer;
        this.levelMusicFlow = levelMusicFlow;
        this.bossEnvironmentManifest = bossEnvironmentManifest;
        this.bossEnvironmentRegistry = bossEnvironmentRegistry;
        this.addressableAssetLoader = addressableAssetLoader;
        this.diContainer = diContainer;
    }

    public virtual async UniTask Initialize(ThoughtType bossType)
    {
        originalSkybox = RenderSettings.skybox;
        addressableKey = bossEnvironmentRegistry.GetReference(bossType);
        var result = await addressableAssetLoader.LoadAsset<GameObject>(addressableKey);

        if (result == null)
            throw new Exception($"Failed to load environment prefab for {bossType}");

        instance = diContainer.InstantiatePrefab(result);

        bossEnvironmentView = instance.GetComponent<IBossEnvironmentView>();
        bossEnvironmentView.ApplySound(audioPlayer);
        bossEnvironmentView.ApplyLighting(lightService);

        await bossEnvironmentView.PlayAnimationAsync();
    }

    public virtual void Cleanup()
    {
        ApplyOriginLight();

        bossEnvironmentView?.StopAnimation();
        levelMusicFlow.PlayMainSoundTrack();

        addressableAssetLoader.Unload(addressableKey).Forget();

        if (instance != null)
            GameObject.Destroy(instance);

        instance = null;
        bossEnvironmentView = null;
    }

    private void ApplyOriginLight()
    {
        lightService.ToOrigin();
    }
}
