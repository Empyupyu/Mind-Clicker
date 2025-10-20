using System;
using UnityEngine;
using Zenject;

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
