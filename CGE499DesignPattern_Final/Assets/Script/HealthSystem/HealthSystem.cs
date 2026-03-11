using System;
using UnityEngine;

public class HealthSystem
{
    public float MaxHP { get; private set; }
    public float CurrentHP { get; private set; }
    public bool IsDead => CurrentHP <= 0;

    public event Action OnDied;
    public event Action<float> OnHealthChanged;

    public HealthSystem(float maxHP)
    {
        MaxHP = maxHP;
        CurrentHP = maxHP;
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        CurrentHP -= damage;
        OnHealthChanged?.Invoke(CurrentHP);

        if (CurrentHP <= 0)
        {
            CurrentHP = 0;
            OnDied?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (IsDead) return;

        CurrentHP += amount;
        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        OnHealthChanged?.Invoke(CurrentHP);
    }

    public void IncreaseMaxHealth(float amount, bool healBonus = true)
    {
        if (amount <= 0f) return;

        MaxHP += amount;

        if (healBonus)
        {
            CurrentHP += amount;
        }

        CurrentHP = Mathf.Min(CurrentHP, MaxHP);
        OnHealthChanged?.Invoke(CurrentHP);
    }

    public void Kill()
    {
        if (IsDead) return;

        CurrentHP = 0;
        OnHealthChanged?.Invoke(CurrentHP);
        OnDied?.Invoke();
    }
}
