using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

public class Phone : MonoBehaviour
{
    [SerializeField] private AudioClip ringClip;
    [SerializeField] private ParticleSystem particles;
    private AudioPlayer audioPlayer;

    [Inject]
    public void Construct(AudioPlayer audioPlayer)
    {
        this.audioPlayer = audioPlayer;
    }

    public async UniTask PlayRoutine(CancellationToken token)
    {
        await UniTask.Delay(2500, cancellationToken: token);

        while (!token.IsCancellationRequested)
        {
            audioPlayer.PlaySFX(ringClip);
            particles.Play();
            await UniTask.Delay(5000, cancellationToken: token);
        }
    }
}