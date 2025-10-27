using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour, IDisposable
{
    public event Action<AudioClip> OnTrackFinished;

    [Header("Audio Setup")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambiantSource;
    [SerializeField] private AudioSource sfxSource;

    [SerializeField] private AudioClip[] musicClips;
    [SerializeField] private AudioClip openingAmbiantClip;
    [SerializeField] private AudioClip gameAmbiantClip;

    [Header("Fade Settings")]
    [SerializeField] private float fadeOutDuration = 1f;

    private CancellationTokenSource fadeToken;
    private CancellationTokenSource sequenceToken;

    public AudioClip[] GetMusicClips() 
    {
        return musicClips;
    }

    public async UniTask Opening()
    {
        var sequence = new List<(AudioClip, float)>
        {
         (openingAmbiantClip, 0.04f),
         (gameAmbiantClip, 1f),
         (gameAmbiantClip, .7f),
        };

        await SequenceMusic(sequence, 7500);
    }

    public void LoopMusicInGame()
    {
        var sequence = new List<(AudioClip, float)>
        {
         (gameAmbiantClip, .01f),
         (gameAmbiantClip, 1f),
         (gameAmbiantClip, .7f),
        };

        SequenceMusic(sequence, 15000).Forget();
    }

    public void SetMusicVolume(float volumePercent)
    {
        musicSource.volume = Mathf.Clamp01(volumePercent);
    }

    public void PlaySFX(AudioClip clip, float volume = 1f)
    {
        if (clip == null || sfxSource == null) return;

        sfxSource.PlayOneShot(clip, Mathf.Clamp01(volume));
    }

    public void Dispose()
    {
        OnTrackFinished = null;
    }

    public void ForceStopAmbients()
    {
        sequenceToken?.Cancel();
        ambiantSource?.Stop();
    }

    private async UniTask SequenceMusic(List<(AudioClip, float)> sequence, int delayBeforePlayMusic)
    {
        PlayAmbientSequence(sequence);

        await UniTask.Delay(delayBeforePlayMusic);

        PlayMusic(musicClips[UnityEngine.Random.Range(0, musicClips.Length)], 1);
    }

    private void PlayAmbientSequence(List<(AudioClip clip, float durationPercent)> sequence)
    {
        if (ambiantSource == null || sequence == null || sequence.Count == 0) return;

        sequenceToken?.Cancel();
        sequenceToken = new CancellationTokenSource();

        PlaySequenceAsync(sequence, sequenceToken.Token).Forget();
    }

    private async UniTaskVoid PlaySequenceAsync(List<(AudioClip clip, float durationPercent)> sequence, CancellationToken token)
    {
        foreach (var (clip, percent) in sequence)
        {
            if (clip == null || token.IsCancellationRequested) break;

            float durationPercent = Mathf.Clamp01(percent);
            ambiantSource.clip = clip;
            ambiantSource.volume = 1f;
            ambiantSource.Play();

            float playDuration = clip.length * durationPercent;
            float waitTime = Mathf.Max(0f, playDuration - fadeOutDuration);

            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);

            float startVolume = ambiantSource.volume;
            float elapsed = 0f;

            while (elapsed < fadeOutDuration)
            {
                if (token.IsCancellationRequested) return;

                elapsed += Time.deltaTime;
                float t = 1f - (elapsed / fadeOutDuration);
                ambiantSource.volume = Mathf.Lerp(0f, startVolume, t);
                await UniTask.Yield();
            }

            ambiantSource.Stop();
            ambiantSource.volume = 1f;
        }
    }

    public void PlayMusic(AudioClip clip, float durationPercent)
    {
        if (clip == null || musicSource == null) return;

        durationPercent = Mathf.Clamp01(durationPercent);
        musicSource.clip = clip;
        musicSource.volume = 1f;
        musicSource.Play();

        float clipDuration = clip.length;
        float playDuration = clipDuration * durationPercent;

        fadeToken?.Cancel();
        fadeToken = new CancellationTokenSource();

        FadeOutAndStopAsync(playDuration, fadeToken.Token).Forget();
    }

    private async UniTaskVoid FadeOutAndStopAsync(float delay, CancellationToken token)
    {
        float waitTime = Mathf.Max(0f, delay - fadeOutDuration);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);

        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeOutDuration)
        {
            if (token.IsCancellationRequested) return;

            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / fadeOutDuration);
            musicSource.volume = Mathf.Lerp(0f, startVolume, t);
            await UniTask.Yield();
        }

        musicSource.Stop();
        musicSource.volume = 1f;

        OnTrackFinished?.Invoke(musicSource.clip);
    }
}
