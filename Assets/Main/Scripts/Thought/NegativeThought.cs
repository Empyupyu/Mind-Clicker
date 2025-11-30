using System;
using UnityEngine;

public class NegativeThought
{
    public event Action<NegativeThought> OnDeath;
    public event Action<NegativeThought> OnHealthChange;
    public string Id { get; }
    public string Name { get; }
    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }
    public float Money { get; }

    public bool IsActive;

    public NegativeThought(string id, string name, float health, float money)
    {
        Id = id;
        Name = name;
        MaxHealth = health;
        CurrentHealth = health;
        Money = money;
    }

    public void ApplyDamage(float damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChange?.Invoke(this);

        if (CurrentHealth > 0) return;

        OnDeath?.Invoke(this);
    }
}