using System;
using System.Collections.Generic;

[Serializable]
public class PlayerData
{
    public float MindPoints;
    public int MindLevel;
    public int TotalMindLevel;
    public int PrestigePoints;

    public int CurrentLevel;

    public float SoftCurrency;
    public float HardCurrency;
    public bool AdvertisementIsDisabled;

    public List<UpgradeProgress> Upgrades;
    public List<MindLevel> MindLevelsProgress;
}
