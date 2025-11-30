using Cysharp.Threading.Tasks;
using DG.Tweening;
using System.Threading;
using UnityEngine;

public class DarknesEnvironmentView : MonoBehaviour, IBossEnvironmentView
{
    [field: SerializeField] public LightConfig LightConfig { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; } = 2f;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }
    [field: SerializeField] public Transform Dog { get; private set; }

    private CancellationTokenSource dogAnimationCts;

    public async UniTask PlayAnimationAsync()
    {
        dogAnimationCts = new CancellationTokenSource();

        SetPropsScale(Dog, Vector3.zero);

        await AnimateProps(Dog, 1f, 0.4f, Ease.OutBack, 200, dogAnimationCts.Token);
    }

    public void ApplyLighting(LightService lightService)
    {
        
    }

    public void ApplySound(AudioPlayer audioPlayer)
    {
        
    }

    public void StopAnimation()
    {
        dogAnimationCts?.Cancel();
        Dog.DOKill();
    }

    private void SetPropsScale(Transform prop, Vector3 scale)
    {
        prop.localScale = scale;
    }

    private async UniTask AnimateProps(Transform prop, float scale, float duration, Ease ease, int delay, CancellationToken cancellationToken)
    {
        if(cancellationToken.IsCancellationRequested) return;

        prop.DOScale(scale, duration).From(0).SetEase(ease);
        await UniTask.Delay(delay);
    }
}
