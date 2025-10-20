using System;
using UnityEngine;

public class NegativeThought
{
    public event Action<NegativeThought> OnDeath;
    public event Action<float> OnHealthChange;
    public string Id { get; }
    public string Name { get; }
    public float MaxHealth { get; }
    public float CurrentHealth { get; private set; }

    public bool IsActive;

    public NegativeThought(string id, string name, float health)
    {
        Id = id;
        Name = name;
        MaxHealth = health;
        CurrentHealth = health;
    }

    public void ApplyDamage(float damage)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        OnHealthChange?.Invoke(CurrentHealth / MaxHealth);

        if (CurrentHealth > 0) return;

        OnDeath?.Invoke(this);
    }
}
