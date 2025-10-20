using System;
using System.Collections.Generic;

public class Mind
{
    public event Action OnLevelUp;
    public event Action OnLevelReduce;

    public float PointForLevelUp { get; private set; }
    public float ProgressPercent => playerData.MindPoints / PointForLevelUp;
    private bool ShouldLevelUp => playerData.MindPoints == PointForLevelUp;

    private readonly PlayerData playerData;
    private readonly MindData mindData;

    public Mind(PlayerData playerData, MindData mindData)
    {
        this.playerData = playerData;
        this.mindData = mindData;

        InitializeProgress();
        ApplyNextTargetMindPoints();
    }

    public void AddMindPoints(float deltaTime)
    {
        playerData.MindPoints += mindData.BaseGainRate * deltaTime;

        if (playerData.MindPoints >= PointForLevelUp)
            playerData.MindPoints = PointForLevelUp;

        if (!ShouldLevelUp) return;

        LevelUp();
    }

    public void LevelUp()
    {
        playerData.MindLevel++;
        playerData.MindPoints = 0;
        ApplyNextTargetMindPoints();

        OnLevelUp?.Invoke();
    }

    public void ReduceLevel()
    {
        playerData.MindLevel--;
        OnLevelReduce?.Invoke();
    }

    private void ApplyNextTargetMindPoints()
    {
        PointForLevelUp = playerData.MindLevel < mindData.MindLevels.Count
            ? mindData.MindLevels[playerData.MindLevel].MindPointsForLevelUp
            : mindData.MindLevels[^1].MindPointsForLevelUp * playerData.MindLevel;
    }

    private void InitializeProgress()
    {
        if (playerData.MindLevelsProgress == null || playerData.MindLevelsProgress.Count == 0)
        {
            playerData.MindLevelsProgress = new List<MindLevel>
            {
                mindData.MindLevels[playerData.MindLevel]
            };
        }
    }
}
