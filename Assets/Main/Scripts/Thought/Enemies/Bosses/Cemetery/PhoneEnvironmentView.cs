using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class PhoneEnvironmentView : MonoBehaviour, IBossEnvironmentView
{
    [field: SerializeField] public LightConfig LightConfig { get; private set; }
    [field: SerializeField] public float TransitionDuration { get; private set; } = 2f;
    [field: SerializeField] public AudioClip AudioClip { get; private set; }
    [field: SerializeField] public Phone Phone { get; private set; }

    private CancellationTokenSource phoneAnimationCts;

    public async UniTask PlayAnimationAsync() 
    {
        phoneAnimationCts = new CancellationTokenSource();
        Phone.PlayRoutine(phoneAnimationCts.Token).Forget();
    }

    public void ApplyLighting(LightService lightService)
    {
   
    }

    public void ApplySound(AudioPlayer audioPlayer)
    {

    }

    public void StopAnimation()
    {
        phoneAnimationCts?.Cancel();
    }
}
