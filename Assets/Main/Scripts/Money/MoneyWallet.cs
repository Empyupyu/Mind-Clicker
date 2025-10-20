using System;

public class MoneyWallet
{
    public event Action<float> OnPut, OnSpend;
    public event Action OnNotEnough;

    private readonly PlayerDataRef playerData;

    public MoneyWallet(PlayerDataRef playerData)
    {
        this.playerData = playerData;
    }

    public bool HasEnough(float price)
    {
        return playerData.Value.Money >= price;
    }

    public float GetAmount()
    {
        return playerData.Value.Money;
    }

    public void Put(float amount)
    {
        playerData.Value.Money += amount;
        OnPut?.Invoke(playerData.Value.Money);
    }

    public void Spend(float amount)
    {
        if (!HasEnough(amount))
        {
            OnNotEnough?.Invoke();
            return;
        }

        playerData.Value.Money -= amount;
        OnSpend?.Invoke(playerData.Value.Money);
    }
}
