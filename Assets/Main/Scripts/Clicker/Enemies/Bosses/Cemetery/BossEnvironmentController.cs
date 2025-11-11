using Cysharp.Threading.Tasks;
using System;
using UnityEngine;
using Zenject;

public class BossEnvironmentController : IBossEnvironmentController
{
    protected readonly BossEnvironmentManifest bossEnvironmentManifest;
    protected readonly BossEnvironmentRegistry bossEnvironmentRegistry;
    protected readonly LightService lightService;
    protected readonly AudioPlayer audioPlayer;
    protected Material originalSkybox;
    protected GameObject instance;
    protected IBossEnvironmentView bossEnvironmentView;

    private readonly AddressableAssetLoader addressableAssetLoader;
    private readonly DiContainer diContainer;
    private string addressableKey;

    public BossEnvironmentController(LightService lightService, AudioPlayer audioPlayer, BossEnvironmentManifest bossEnvironmentManifest, BossEnvironmentRegistry bossEnvironmentRegistry, AddressableAssetLoader addressableAssetLoader, DiContainer diContainer)
    {
        this.lightService = lightService;
        this.audioPlayer = audioPlayer;
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

        bossEnvironmentView.StopAnimation();
        audioPlayer.PlayMainSoundTrack();

        addressableAssetLoader.Unload(addressableKey).Forget();

        if (instance != null)
            GameObject.Destroy(instance);

        instance = null;
        bossEnvironmentView = null;
    }

    private void ApplyOriginLight()
    {
        lightService.ToOriginEnvironmentLighting();
        lightService.ToOriginSunRotate(GetTransitionDuration());
        lightService.ChangeSkyBox(originalSkybox, GetTransitionDuration());
    }

    protected float GetTransitionDuration()
    {
        return bossEnvironmentView.TransitionDuration;
    }
}
