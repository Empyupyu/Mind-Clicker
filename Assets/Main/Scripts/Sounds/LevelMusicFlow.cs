using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;

public class LevelMusicFlow : IInitializable, IDisposable
{
    private readonly AudioPlayer audioPlayer;
    private readonly AudioConfig audioConfig;

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
        var sequence = new List<(AudioClip, float)>
        {
         (audioConfig.OpeningAmbiantClip, audioConfig.OpeningAmbiantClipStartTime),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartFull),
         (audioConfig.LevelAmbiantClip, audioConfig.LevelAmbiantStartMid),
        };

        await audioPlayer.SequenceMusic(sequence, audioConfig.OpeningDelay);
    }

    public void LoopMusicInGame()
    {
        var sequence = CreateMainAmbiantSequence();

        audioPlayer.SequenceMusic(sequence, audioConfig.LoopMusicDelay).Forget();
    }

    public void PlayMainSoundTrack()
    {
        var sequence = CreateMainAmbiantSequence();

        audioPlayer.SequenceMusic(sequence, 0).Forget();
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
    }
}