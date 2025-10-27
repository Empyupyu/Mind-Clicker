using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelState : IGameState
{
    private readonly GameFlowController flowController;
    private readonly AudioPlayer audioPlayer;
    private readonly GameData gameData;

    public LevelState(GameFlowController flowController, AudioPlayer audioPlayer, GameData gameData)
    {
        this.flowController = flowController;
        this.audioPlayer = audioPlayer;
        this.gameData = gameData;
    }

    public async UniTask Enter()
    {
        audioPlayer.OnTrackFinished += MusicFinished;
        await audioPlayer.Opening();

        await UniTask.Delay(1000);
        gameData.Level.Container.Resolve<ThoughtSpawner>().Spawn();
    }

    private void MusicFinished(AudioClip clip)
    {
        audioPlayer.LoopMusicInGame();
    }

    public UniTask Exit()
    {
        audioPlayer.OnTrackFinished -= MusicFinished;
        return UniTask.CompletedTask;
    }
}
