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
        return playerData.Value.SoftCurrency >= price;
    }

    public float GetAmount()
    {
        return playerData.Value.SoftCurrency;
    }

    public void Put(float amount)
    {
        playerData.Value.SoftCurrency += amount;
        OnPut?.Invoke(playerData.Value.SoftCurrency);
    }

    public void Spend(float amount)
    {
        if (!HasEnough(amount))
        {
            OnNotEnough?.Invoke();
            return;
        }

        playerData.Value.SoftCurrency -= amount;
        OnSpend?.Invoke(playerData.Value.SoftCurrency);
    }
}
