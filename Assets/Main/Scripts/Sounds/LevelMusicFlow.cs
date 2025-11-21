using Cysharp.Threading.Tasks;
using UnityEngine;
using System.Collections.Generic;
using Zenject;
using System;

//TODO
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
         (audioConfig.OpeningAmbiantClip, 0.04f),
         (audioConfig.LevelAmbiantClip, 1f),
         (audioConfig.LevelAmbiantClip, .7f),
        };

        await audioPlayer.SequenceMusic(sequence, 7500);
    }

    public void LoopMusicInGame()
    {
        var sequence = CreateMainAmbiantSequence();

        audioPlayer.SequenceMusic(sequence, 15000).Forget();
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
            (audioConfig.LevelAmbiantClip, .01f),
         (audioConfig.LevelAmbiantClip, 1f),
         (audioConfig.LevelAmbiantClip, .7f),
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