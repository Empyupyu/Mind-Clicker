using System;
using UnityEngine;
using Zenject;

public class MoneyWallet
{
    public event Action<float> OnPut, OnSpend;
    public event Action OnNotEnough;

    private readonly PlayerData playerData;

    public MoneyWallet(PlayerData playerData)
    {
        this.playerData = playerData;
    }

    public bool HasEnough(float price)
    {
        return playerData.Money >= price;
    }

    public float GetAmount()
    {
        return playerData.Money;
    }

    public void Put(float amount)
    {
        playerData.Money += amount;
        OnPut?.Invoke(playerData.Money);
    }

    public void Spend(float amount)
    {
        if (!HasEnough(amount))
        {
            OnNotEnough?.Invoke();
            return;
        }

        playerData.Money -= amount;
        OnSpend?.Invoke(playerData.Money);
    }
}

public class MoneyController : IInitializable, IDisposable
{
    private readonly MoneyWallet wallet;
    private MoneyView moneyView;

    public MoneyController(MoneyWallet wallet, MoneyView moneyView)
    {
        this.wallet = wallet;
        this.moneyView = moneyView;
    }

    public void Initialize()
    {
        moneyView = GameObject.Instantiate(moneyView);
        RedrawWallet(wallet.GetAmount());
        wallet.OnPut += RedrawWallet;
        wallet.OnSpend += RedrawWallet;
    }

    private void RedrawWallet(float amount)
    {
        moneyView.MoneyText.text = string.Format("{0:0.0}", amount);
    }

    public void Dispose()
    {    
        wallet.OnPut -= RedrawWallet;
        wallet.OnSpend -= RedrawWallet;
    }
}

public class ThoughtRewardHandler : IInitializable, IDisposable
{
    private readonly MoneyWallet wallet;
    private readonly ThoughtSpawner thoughtSpawner;

    public ThoughtRewardHandler(MoneyWallet wallet, ThoughtSpawner thoughtSpawner)
    {
        this.wallet = wallet;
        this.thoughtSpawner = thoughtSpawner;
    }

    public void Initialize()
    {
        thoughtSpawner.OnDestroy += AddMoney;
    }

    private void AddMoney(NegativeThought negativeThought)
    {
        wallet.Put(negativeThought.Money);
    }

    public void Dispose()
    {
        thoughtSpawner.OnDestroy -= AddMoney;
    }
}