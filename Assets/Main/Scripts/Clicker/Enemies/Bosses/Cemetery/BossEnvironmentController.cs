using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class BossEnvironmentController : IBossEnvironmentController
{
    protected readonly BossEnvironmentManifest bossEnvironmentManifest;
    protected readonly BossEnvironmentRegistry bossEnvironmentRegistry;
    protected readonly LightService lightService;
    protected readonly AudioPlayer audioPlayer;
    protected Material originalSkybox;
    protected GameObject instance;
    protected IBossEnvironmentView bossEnvironmentView;

    public BossEnvironmentController(LightService lightService, AudioPlayer audioPlayer, BossEnvironmentManifest bossEnvironmentManifest, BossEnvironmentRegistry bossEnvironmentRegistry)
    {
        this.lightService = lightService;
        this.audioPlayer = audioPlayer;
        this.bossEnvironmentManifest = bossEnvironmentManifest;
        this.bossEnvironmentRegistry = bossEnvironmentRegistry;
    }

    public virtual async UniTask Initialize(ThoughtType bossType)
    {
        originalSkybox = RenderSettings.skybox;

        var handle = Addressables.LoadAssetAsync<GameObject>(bossEnvironmentRegistry.GetReference(bossType));
        await handle.ToUniTask();

        var prefab = handle.Result;
        instance = GameObject.Instantiate(prefab);
        bossEnvironmentView = instance.GetComponent<IBossEnvironmentView>();

        ApplyLighting();
        ApplySound();

        await bossEnvironmentView.ApplyAnimation();
    }

    public virtual void Cleanup()
    {
        lightService.ToOriginEnvironmentLighting();
        lightService.ToOriginSunRotate(GetTransitionDuration());
        lightService.ChangeSkyBox(originalSkybox, GetTransitionDuration());
        audioPlayer.PlayMainSoundTrack();

        if (instance != null)
            GameObject.Destroy(instance);
    }

    protected void ApplySound()
    {
        audioPlayer.ForceStopAmbients();
        audioPlayer.PlayMusic(bossEnvironmentView.AudioClip, 1f);
    }

    protected void ApplyLighting()
    {
        lightService.SetEnvironmentLighting(bossEnvironmentView.LightConfig);
        lightService.SetLightIntensity(0.3f, GetTransitionDuration());
        lightService.SetSunRotate(new Vector2(-4f, 200), GetTransitionDuration());
        lightService.ChangeSkyBox(bossEnvironmentView.LightConfig.Skybox, GetTransitionDuration());
    }

    protected float GetTransitionDuration()
    {
        return bossEnvironmentView.TransitionDuration;
    }
}
