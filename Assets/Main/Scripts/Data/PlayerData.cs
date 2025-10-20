using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public float MindPoints;
    public int MindLevel;

    public int CurrentLevel;

    public float Money;

    public List<UpgradeProgress> Upgrades;
    public List<MindLevel> MindLevelsProgress;
}
