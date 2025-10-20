using System;
using Zenject;

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