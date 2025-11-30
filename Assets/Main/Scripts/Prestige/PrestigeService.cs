using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PrestigeService : ITickable
{
    private readonly PlayerDataRef playerDataRef;
    private readonly MoneyWallet moneyWallet;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly SignalBus signalBus;
    private readonly LightService lightService;
    private readonly UpgradeService upgradeService;
    private readonly UpgradeController upgradeController;

    public PrestigeService(
        PlayerDataRef playerDataRef,
        MoneyWallet moneyWallet,
        ThoughtSpawner thoughtSpawner, 
        SignalBus signalBus,
        LightService lightService,
        UpgradeService upgradeService,
        UpgradeController upgradeController)
    {
        this.playerDataRef = playerDataRef;
        this.moneyWallet = moneyWallet;
        this.thoughtSpawner = thoughtSpawner;
        this.signalBus = signalBus;
        this.lightService = lightService;
        this.upgradeService = upgradeService;
        this.upgradeController = upgradeController;
    }

    public void DoPrestige()
    {
        playerDataRef.Value.PrestigePoints += CalculatePrestigeGain();

        playerDataRef.Value.MindLevel = 0;
        playerDataRef.Value.MindLevelsProgress = new List<MindLevel>();
        playerDataRef.Value.Upgrades = new List<UpgradeProgress>();
        playerDataRef.Value.CurrentLevel = 0;

        moneyWallet.Spend(moneyWallet.GetAmount());

        Debug.Log("Presitge points: " + playerDataRef.Value.PrestigePoints);

        thoughtSpawner.DestroyAll();

        lightService.ToOrigin();

        upgradeService.Initialize();
        upgradeController.RedrawViews();

        signalBus.Fire(new GameLoadedSignal());
    }

    public void Tick()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            DoPrestige();
        }
    }

    private int CalculatePrestigeGain()
    {
        return Mathf.FloorToInt(Mathf.Sqrt(playerDataRef.Value.MindLevel));
    }
}
