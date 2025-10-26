using Main.Scripts.Views;
using System;
using UnityEngine;

public class UpgradeStateView : MonoBehaviour
{
    public UpgradeViewState CurrentState => currentState;

    [SerializeField] private GameObject blockIcon;
    [SerializeField] private GameObject comingSoonIcon;
    [SerializeField] private BuyUpgradeView buyUpgradeView;

    private UpgradeViewState currentState;

    public void SetState(UpgradeViewState state)
    {
        currentState = state;

        blockIcon.SetActive(state == UpgradeViewState.Locked);
        comingSoonIcon.SetActive(state == UpgradeViewState.ComingSoon);
        buyUpgradeView.gameObject.SetActive(state == UpgradeViewState.Unlocked);

        if (state == UpgradeViewState.Unlocked)
        {
            
        }

        if (state != UpgradeViewState.Unlocked)
        {
            buyUpgradeView.Description.text = "";
            buyUpgradeView.Level.text = "";
            buyUpgradeView.Price.text = "";
        }
    }

    public void UpdateContent(string desc, int level, float price)
    {
        if (currentState != UpgradeViewState.Unlocked)
            return;

        buyUpgradeView.Description.text = desc;
        buyUpgradeView.Level.gameObject.SetActive(level > 0);
        buyUpgradeView.Level.text = $"Level: {level}";
        buyUpgradeView.Price.text = $"{price}$";
    }

    public void SetIcon(Sprite icon)
    {
        buyUpgradeView.Icon.sprite = icon;
    }

    public void SubscribeToBuy(Action onClick)
    {
        buyUpgradeView.Buy.onClick.RemoveAllListeners();
        buyUpgradeView.Buy.onClick.AddListener(() => onClick?.Invoke());
    }

    private void OnDisable()
    {
        buyUpgradeView.Buy.onClick.RemoveAllListeners();
    }
}
