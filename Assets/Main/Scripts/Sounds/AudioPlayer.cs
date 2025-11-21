using Cysharp.Threading.Tasks;
using System.Threading;
using System;
using UnityEngine;
using System.Collections.Generic;
using Zenject;

public class AudioPlayer : MonoBehaviour
{
    public event Action<AudioClip> OnTrackFinished;

    [Header("Audio Setup")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource ambiantSource;
    [SerializeField] private AudioSource sfxSource;

    private AudioConfig audioConfig;
    private CancellationTokenSource fadeToken;
    private CancellationTokenSource sequenceToken;
    private SignalBus signalBus;
    private const string VolumeParam = "MasterVolume";

    [Inject]
    public void Construct(SignalBus signalBus, AudioConfig audioConfig)
    {
        this.signalBus = signalBus;
        this.audioConfig = audioConfig;
    }

    private void OnEnable()
    {
        signalBus.Subscribe<SoundEffectSignal>(OnSoundSignal);
        signalBus.Subscribe<MuteSoundsSignal>(MuteSounds);
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

    public void ForceStopAmbients()
    {
        sequenceToken?.Cancel();
        ambiantSource?.Stop();
    }

    public async UniTask SequenceMusic(List<(AudioClip, float)> sequence, int delayBeforePlayMusic)
    {
        PlayAmbientSequence(sequence);

        await UniTask.Delay(delayBeforePlayMusic);

        PlayMusic(audioConfig.MusicClips[UnityEngine.Random.Range(0, audioConfig.MusicClips.Length)], 1);
    }

    private void PlayAmbientSequence(List<(AudioClip clip, float durationPercent)> sequence)
    {
        if (ambiantSource == null || sequence == null || sequence.Count == 0) return;

        sequenceToken?.Cancel();
        sequenceToken = new CancellationTokenSource();

        PlaySequenceAsync(sequence, sequenceToken.Token).Forget();
    }

    private void MuteSounds(MuteSoundsSignal muteSoundsSignal)
    {
#if UNITY_EDITOR
        Debug.Log("Muted sounds is [" + muteSoundsSignal.Muted + "]");
#endif

        if (muteSoundsSignal.Muted)
        {
            musicSource.Pause();
            ambiantSource.Pause();
            musicSource.outputAudioMixerGroup.audioMixer.SetFloat(VolumeParam, -80f);
            return;
        }

        musicSource.UnPause();
        ambiantSource.UnPause();
        musicSource.outputAudioMixerGroup.audioMixer.SetFloat(VolumeParam, 0f);
    }

    private void OnSoundSignal(SoundEffectSignal signal)
    {
        PlaySFX(signal.AudioClip, signal.Volume);
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
            float waitTime = Mathf.Max(0f, playDuration - audioConfig.FadeOutDuration);

            await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);

            float startVolume = ambiantSource.volume;
            float elapsed = 0f;

            while (elapsed < audioConfig.FadeOutDuration)
            {
                if (token.IsCancellationRequested) return;

                elapsed += Time.deltaTime;
                float t = 1f - (elapsed / audioConfig.FadeOutDuration);
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
        float waitTime = Mathf.Max(0f, delay - audioConfig.FadeOutDuration);
        await UniTask.Delay(TimeSpan.FromSeconds(waitTime), cancellationToken: token);

        float startVolume = musicSource.volume;
        float elapsed = 0f;

        while (elapsed < audioConfig.FadeOutDuration)
        {
            if (token.IsCancellationRequested) return;

            elapsed += Time.deltaTime;
            float t = 1f - (elapsed / audioConfig.FadeOutDuration);
            musicSource.volume = Mathf.Lerp(0f, startVolume, t);
            await UniTask.Yield();
        }

        musicSource.Stop();
        musicSource.volume = 1f;

        OnTrackFinished?.Invoke(musicSource.clip);
    }

    private void OnDisable()
    {
        OnTrackFinished = null;
        signalBus.Unsubscribe<SoundEffectSignal>(OnSoundSignal);
        signalBus.Unsubscribe<MuteSoundsSignal>(MuteSounds);
        fadeToken?.Cancel();
        fadeToken?.Dispose();
        sequenceToken?.Cancel();
        sequenceToken?.Dispose();
    }
}