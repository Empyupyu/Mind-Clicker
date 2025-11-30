using Cysharp.Threading.Tasks;
using System;
using YG;
using Zenject;

public interface IGameModule
{
    int Priority { get; }
    UniTask InitializeAsync();
}

//TODO
public interface ILeaderboard
{

}


public class YandexLeaderboard : ILeaderboard, IInitializable, IDisposable
{
    private readonly MindProgress mindProgress;
    private readonly PlayerDataRef playerData;

    public YandexLeaderboard(
        MindProgress mindProgress,
        PlayerDataRef playerData)
    {
        this.mindProgress = mindProgress;
        this.playerData = playerData;
    }

    public void Initialize()
    {
        mindProgress.OnLevelUp += UpdateRecord;
    }

    public void SetNewRecord(int score)
    {
        YG2.SetLeaderboard("LevelLeaderBoard", score);
    }

    public void Dispose()
    {
        mindProgress.OnLevelUp -= UpdateRecord;
    }

    private void UpdateRecord()
    {
        SetNewRecord(playerData.Value.TotalMindLevel);
    }
}