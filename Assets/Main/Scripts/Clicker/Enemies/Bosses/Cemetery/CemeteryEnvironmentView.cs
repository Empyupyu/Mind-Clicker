using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CemeteryEnvironmentView : MonoBehaviour, IBossEnvironmentView
{
    [field: SerializeField] public LightConfig LightConfig { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; } = 2f;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }
    [field: SerializeField] public List<Transform> Ghosts { get; private set; }
    [field: SerializeField] public List<Transform> Gravestones { get; private set; }
    [field: SerializeField] public List<Transform> Mushrooms { get; private set; }

    private CancellationTokenSource mushroomsAnimationCts;
    private CancellationTokenSource gravestonesAnimationCts;

    public async UniTask PlayAnimationAsync()
    {
        SetPropsScale(Mushrooms, Vector3.zero);
        SetPropsScale(Gravestones, Vector3.zero);
        EnableGhosts(false);

        mushroomsAnimationCts = new CancellationTokenSource();
        gravestonesAnimationCts = new CancellationTokenSource();

        await AnimateProps(Mushrooms, 1f, 0.4f, Ease.OutBack, 200, mushroomsAnimationCts.Token);
        await AnimateProps(Gravestones, 1f, 1f, Ease.OutBack, 500, gravestonesAnimationCts.Token);

        EnableGhosts(true);
    }

    public void ApplySound(AudioPlayer audioPlayer)
    {
        audioPlayer.ForceStopAmbients();
        audioPlayer.PlayMusic(AudioClip, 1f);
    }

    public void ApplyLighting(LightService lightService)
    {
        lightService.SetEnvironmentLighting(LightConfig);
        lightService.SetLightIntensity(0.3f, TransitionDuration);
        lightService.SetSunRotate(new Vector2(-4f, 200), TransitionDuration);
        lightService.ChangeSkyBox(LightConfig.Skybox, TransitionDuration);
    }

    public void StopAnimation()
    {
        mushroomsAnimationCts?.Cancel();
        gravestonesAnimationCts?.Cancel();

        KillTweens(Mushrooms);
        KillTweens(Gravestones);
    }

    private void EnableGhosts(bool active)
    {
        foreach (var ghost in Ghosts)
            ghost.gameObject.SetActive(active);
    }

    private void SetPropsScale(List<Transform> props, Vector3 scale)
    {
        foreach (var prop in props)
            prop.localScale = scale;
    }

    private async UniTask AnimateProps(List<Transform> props, float scale, float duration, Ease ease, int delay, CancellationToken token)
    {
        foreach (var prop in props)
        {
            if (token.IsCancellationRequested) return;

            prop.DOScale(scale, duration).From(0).SetEase(ease);
            await UniTask.Delay(delay);
        }
    }

    private void KillTweens(List<Transform> props)
    {
        foreach (var prop in props)
            prop.DOKill();
    }
}
