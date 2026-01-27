using System.Collections.Generic;
using UnityEngine;
using Zenject;
using static YG.YG2;

public class PrestigeService : ITickable
{
    private readonly PlayerDataRef playerDataRef;
    private readonly MoneyWallet moneyWallet;
    private readonly ThoughtSpawner thoughtSpawner;
    private readonly SignalBus signalBus;
    private readonly LightService lightService;
    private readonly UpgradeService upgradeService;
    private readonly UpgradeController upgradeController;
    private readonly IPristigeCalculate pristigeCalculate;

    public PrestigeService(
        PlayerDataRef playerDataRef,
        MoneyWallet moneyWallet,
        ThoughtSpawner thoughtSpawner, 
        SignalBus signalBus,
        LightService lightService,
        UpgradeService upgradeService,
        UpgradeController upgradeController,
        IPristigeCalculate pristigeCalculate,
        BossEnvironmentController bossEnvironmentController,
        BossFightPrepare bossFightPrepare)
    {
        this.playerDataRef = playerDataRef;
        this.moneyWallet = moneyWallet;
        this.thoughtSpawner = thoughtSpawner;
        this.signalBus = signalBus;
        this.lightService = lightService;
        this.upgradeService = upgradeService;
        this.upgradeController = upgradeController;
        this.pristigeCalculate = pristigeCalculate;
    }

    public void ResetProgress()
    {
        AddPristige();
        ResetPlayerData();
        ResetWallet();
        Debug.Log("Presitge points: " + playerDataRef.Value.PrestigePoints);
        ResetLevel();
        ResetSystems();
    }

    private void AddPristige()
    {
        playerDataRef.Value.PrestigePoints += Calculate();
    }

    private void ResetSystems()
    {
        upgradeService.Initialize();
        upgradeController.Initialize();

        signalBus.Fire(new PrestigeSignal());
        signalBus.Fire(new GameLoadedSignal());
    }

    private void ResetLevel()
    {
        thoughtSpawner.DestroyAll();
        lightService.ToOrigin();
    }

    private void ResetWallet()
    {
        moneyWallet.Spend(moneyWallet.GetAmount());
    }

    private void ResetPlayerData()
    {
        playerDataRef.Value.MindLevel = 0;
        playerDataRef.Value.MindPoints = 0;
        playerDataRef.Value.MindLevelsProgress = new List<MindLevel>();
        playerDataRef.Value.Upgrades = new List<UpgradeProgress>();
        playerDataRef.Value.CurrentLevel = 0;
    }

    public void Tick()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            ResetProgress();
        }
    }

    private int Calculate()
    {
        return pristigeCalculate.Calculate(playerDataRef.Value.MindLevel);
    }
}
