using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using RenderSettings = UnityEngine.RenderSettings;

public class BossCemeteryThoughtLogic : BossThoughtLogicBase
{
    private readonly CemeteryEnvironmentView cemeteryEnvironmentView;
    private readonly LightService lightService;
    private readonly AudioPlayer audioPlayer;
    private CemeteryEnvironmentView instanceEnvironmentView;
    private Material currentSkybox;

    public BossCemeteryThoughtLogic(ThoughtType thoughtType, BossFightPrepare bossFightPrepare, CemeteryEnvironmentView cemeteryEnvironmentView, LightService lightService, AudioPlayer audioPlayer)
        : base(thoughtType, bossFightPrepare)
    {
        this.cemeteryEnvironmentView = cemeteryEnvironmentView;
        this.lightService = lightService;
        this.audioPlayer = audioPlayer;
    }

    public override void AttachTo(NegativeThought thought)
    {
        currentSkybox = RenderSettings.skybox;
        base.AttachTo(thought);

        InitializeEnvironment().Forget();
    }

    private async UniTask InitializeEnvironment()
    {
        instanceEnvironmentView = GameObject.Instantiate(cemeteryEnvironmentView);
        EnableGhosts(false);

        ApplySound();
        ApplyLight();

        SetPropsScale(instanceEnvironmentView.Mushrooms, Vector3.zero);
        SetPropsScale(instanceEnvironmentView.Gravestones, Vector3.zero);

        await PropsScaleAnimation(instanceEnvironmentView.Mushrooms, 1, .4f, Ease.OutBack, 200);
        await PropsScaleAnimation(instanceEnvironmentView.Gravestones, 1, 1f, Ease.OutBack, 500);

        EnableGhosts(true);

        bossFightPrepare.StartFight();
    }

    private void ApplySound()
    {
        audioPlayer.ForceStopAmbients();
        audioPlayer.PlayMusic(instanceEnvironmentView.AudioClip, 1f);
    }

    private async UniTask PropsScaleAnimation(List<Transform> props, float scale, float duration, Ease ease, int delay)
    {
        foreach (var prop in props)
        {
            prop.DOScale(scale, duration).From(0).SetEase(ease);

            await UniTask.Delay(delay);
        }
    }

    private void SetPropsScale(List<Transform> props, Vector3 scale)
    {
        foreach (var prop in props)
        {
            prop.localScale = scale;
        }
    }

    private void ApplyLight()
    {
        lightService.SetEnvironmentLighting(cemeteryEnvironmentView.LightConfig);
        lightService.SetLightIntensity(.3f, instanceEnvironmentView.Duration);
        lightService.SetSunRotate(new Vector2(-4f, 200), instanceEnvironmentView.Duration);
        lightService.ChangeSkyBox(instanceEnvironmentView.LightConfig.Skybox, instanceEnvironmentView.Duration);
    }

    private void EnableGhosts(bool isActive)
    {
        foreach (var ghost in instanceEnvironmentView.Ghosts)
        {
            ghost.gameObject.SetActive(isActive);
        }
    }

    protected override void OnTimerFinished()
    {
        lightService.ToOriginEnvironmentLighting();
        lightService.ToOriginSunRotate(instanceEnvironmentView.Duration);
        lightService.ChangeSkyBox(currentSkybox, instanceEnvironmentView.Duration);
        audioPlayer.PlayMainSoundTrack();
        GameObject.Destroy(instanceEnvironmentView.gameObject);
    }

    protected override void OnBossDeath(NegativeThought negativeThought)
    {
        base.OnBossDeath(negativeThought);
        lightService.ToOriginEnvironmentLighting();
        lightService.ToOriginSunRotate(instanceEnvironmentView.Duration);
        lightService.ChangeSkyBox(currentSkybox, instanceEnvironmentView.Duration);
        audioPlayer.PlayMainSoundTrack();
        GameObject.Destroy(instanceEnvironmentView.gameObject);
    }
}
