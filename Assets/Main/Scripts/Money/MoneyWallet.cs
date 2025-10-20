using System;

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
