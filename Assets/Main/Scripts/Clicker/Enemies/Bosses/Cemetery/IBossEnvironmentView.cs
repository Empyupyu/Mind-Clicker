using Cysharp.Threading.Tasks;
using UnityEngine;

public interface IBossEnvironmentView
{
    public LightConfig LightConfig { get;}
    public float TransitionDuration { get;}
    public AudioClip AudioClip { get;}

    public UniTask ApplyAnimation();
}
