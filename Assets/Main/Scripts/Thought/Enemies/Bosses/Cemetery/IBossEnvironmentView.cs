using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBossEnvironmentView
{
    public LightConfig LightConfig { get;}
    public float TransitionDuration { get;}
    public AudioClip AudioClip { get;}

    public UniTask PlayAnimationAsync();
    public void StopAnimation();
    public void ApplyLighting(LightService lightService);
    public void ApplySound(AudioPlayer audioPlayer);
}
