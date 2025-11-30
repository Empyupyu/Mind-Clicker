using System;
using System.Collections.Generic;

public class MindProgress
{
    public event Action OnLevelUp;
    public event Action OnLevelReduce;

    public float PointForLevelUp { get; private set; }
    public float ProgressPercent => playerData.Value.MindPoints / PointForLevelUp;
    public int Level => playerData.Value.MindLevel;
    private bool ShouldLevelUp => playerData.Value.MindPoints == PointForLevelUp;

    private readonly PlayerDataRef playerData;
    private readonly MindData mindData;

    public MindProgress(PlayerDataRef playerData, MindData mindData)
    {
        this.playerData = playerData;
        this.mindData = mindData;

        InitializeProgress();
        ApplyNextTargetMindPoints();
    }

    public void AddMindPoints(float deltaTime)
    {
        playerData.Value.MindPoints += mindData.BaseGainRate * deltaTime;

        if (playerData.Value.MindPoints >= PointForLevelUp)
            playerData.Value.MindPoints = PointForLevelUp;

        if (!ShouldLevelUp) return;

        LevelUp();
    }

    public void LevelUp()
    {
        playerData.Value.MindLevel++;
        playerData.Value.TotalMindLevel++;
        playerData.Value.MindPoints = 0;
        ApplyNextTargetMindPoints();

        OnLevelUp?.Invoke();
    }

    public void ReduceLevel()
    {
        playerData.Value.MindLevel--;
        OnLevelReduce?.Invoke();
    }

    private void ApplyNextTargetMindPoints()
    {
        PointForLevelUp = playerData.Value.MindLevel < mindData.MindLevels.Count
            ? mindData.MindLevels[playerData.Value.MindLevel].MindPointsForLevelUp
            : mindData.MindLevels[^1].MindPointsForLevelUp * playerData.Value.MindLevel;
    }

    private void InitializeProgress()
    {
        if (playerData.Value.MindLevelsProgress == null || playerData.Value.MindLevelsProgress.Count == 0)
        {
            playerData.Value.MindLevelsProgress = new List<MindLevel>
            {
                mindData.MindLevels[playerData.Value.MindLevel]
            };
        }
    }
}
