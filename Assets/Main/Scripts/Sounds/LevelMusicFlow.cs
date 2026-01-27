using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;
using System.Threading;

public class LevelMusicFlow : IInitializable, IDisposable
{
    private readonly AudioPlayer audioPlayer;
    private readonly AudioConfig audioConfig;
    private List<(AudioClip, float)> openingSequence;
    private CancellationTokenSource musicSequnceCts;

    public LevelMusicFlow(AudioPlayer audioPlayer, AudioConfig audioConfig)
    {
        this.audioPlayer = audioPlayer;
        this.audioConfig = audioConfig;
    }

    public void Initialize()
    {
        audioPlayer.OnTrackFinished += MusicFinished;
    }

    public async UniTask Opening()
    {
        CreateOpeningSequence();

        await audioPlayer.SequenceMusic(openingSequence, audioConfig.OpeningDelay, musicSequnceCts.Token);
    }

    private void CreateOpeningSequence()
    {
        openingSequence = new List<(AudioClip, float)>
        {
         (audioConfig.OpeningAmbiantClip, audioConfig.OpeningAmbiantClipStartTime),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartFull),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartMid),
        };
    }

    public void LoopMusicInGame()
    {
        CancelMusicSequence();
        var sequence = CreateMainAmbiantSequence();

        audioPlayer.SequenceMusic(sequence, audioConfig.LoopMusicDelay, musicSequnceCts.Token).Forget();
    }

    public void PlayMainSoundTrack()
    {
        CancelMusicSequence();
        var sequence = CreateMainAmbiantSequence();

        audioPlayer.SequenceMusic(sequence, 0, musicSequnceCts.Token).Forget();
    }

    public void StopMainSoundTrack()
    {
        CancelMusicSequence();
        openingSequence?.Clear();
        audioPlayer.Stop();
    }

    private void CancelMusicSequence()
    {
        musicSequnceCts?.Cancel();
        musicSequnceCts?.Dispose();
        musicSequnceCts = new CancellationTokenSource();
    }

    private List<(AudioClip, float)> CreateMainAmbiantSequence()
    {
        return new List<(AudioClip, float)>
        {
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartEarly),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartFull),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartMid),
        };
    }

    private void MusicFinished(AudioClip clip)
    {
        LoopMusicInGame();
    }

    public void Dispose()
    {
        audioPlayer.OnTrackFinished -= MusicFinished;

        musicSequnceCts?.Cancel();
        musicSequnceCts?.Dispose();
    }
}