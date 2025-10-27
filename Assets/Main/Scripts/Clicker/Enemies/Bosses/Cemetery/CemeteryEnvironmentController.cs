using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using RenderSettings = UnityEngine.RenderSettings;

public class CemeteryEnvironmentController
{
    private readonly CemeteryEnvironmentView prefab;
    private readonly LightService lightService;
    private readonly AudioPlayer audioPlayer;

    private CemeteryEnvironmentView instance;
    private Material originalSkybox;

    public CemeteryEnvironmentController(CemeteryEnvironmentView prefab, LightService lightService, AudioPlayer audioPlayer)
    {
        this.prefab = prefab;
        this.lightService = lightService;
        this.audioPlayer = audioPlayer;
    }

    public async UniTask Initialize()
    {
        originalSkybox = RenderSettings.skybox;
        instance = GameObject.Instantiate(prefab);

        SetPropsScale(instance.Mushrooms, Vector3.zero);
        SetPropsScale(instance.Gravestones, Vector3.zero);
        EnableGhosts(false);

        ApplyLighting();
        ApplySound();

        await AnimateProps(instance.Mushrooms, 1f, 0.4f, Ease.OutBack, 200);
        await AnimateProps(instance.Gravestones, 1f, 1f, Ease.OutBack, 500);

        EnableGhosts(true);
    }

    public void Cleanup()
    {
        lightService.ToOriginEnvironmentLighting();
        lightService.ToOriginSunRotate(instance.Duration);
        lightService.ChangeSkyBox(originalSkybox, instance.Duration);
        audioPlayer.PlayMainSoundTrack();

        if (instance != null)
            GameObject.Destroy(instance.gameObject);
    }

    private void ApplyLighting()
    {
        lightService.SetEnvironmentLighting(prefab.LightConfig);
        lightService.SetLightIntensity(0.3f, instance.Duration);
        lightService.SetSunRotate(new Vector2(-4f, 200), instance.Duration);
        lightService.ChangeSkyBox(prefab.LightConfig.Skybox, instance.Duration);
    }

    private void ApplySound()
    {
        audioPlayer.ForceStopAmbients();
        audioPlayer.PlayMusic(instance.AudioClip, 1f);
    }

    private void EnableGhosts(bool active)
    {
        foreach (var ghost in instance.Ghosts)
            ghost.gameObject.SetActive(active);
    }

    private void SetPropsScale(List<Transform> props, Vector3 scale)
    {
        foreach (var prop in props)
            prop.localScale = scale;
    }

    private async UniTask AnimateProps(List<Transform> props, float scale, float duration, Ease ease, int delay)
    {
        foreach (var prop in props)
        {
            prop.DOScale(scale, duration).From(0).SetEase(ease);
            await UniTask.Delay(delay);
        }
    }
}
