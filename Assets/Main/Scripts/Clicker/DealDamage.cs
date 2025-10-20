using System;
using UnityEngine;
using Zenject;

public class DealDamage : ITickable
{
    public event Action OnClickDamage;

    private readonly ThoughtSpawner thoughtSpawner;
    private readonly GameData gameData;
    private readonly PlayerData playerData;

    public DealDamage(PlayerData playerData, ThoughtSpawner thoughtSpawner, GameData gameData)
    {
        this.thoughtSpawner = thoughtSpawner;
        this.gameData = gameData;
        this.playerData = playerData;
    }

    public void Tick()
    {
        DamagePerClick();
        DamagePerSecond();
    }

    private void DamagePerClick()
    {
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out RaycastHit hit)) return;

        var view = hit.collider.GetComponentInChildren<ThoughtUIView>();
        if (view == null) return;

        var thought = view.Thought;
        if (thought == null) return;
        if (!thought.IsActive) return;

        thought.ApplyDamage(gameData.DamagePerClick);
        OnClickDamage?.Invoke();
    }

    private void DamagePerSecond()
    {
        if (gameData.DamagePerSecond == 0) return;

        NegativeThought thought = thoughtSpawner.GetTargetThought();

        if (thought == null) return;
        if (!thought.IsActive) return;

        thoughtSpawner.GetTargetThought().ApplyDamage(gameData.DamagePerSecond * Time.deltaTime);
    }
}
