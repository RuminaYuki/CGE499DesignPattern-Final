using System;
using UnityEngine;

public class Brick : MonoBehaviour,IDamageable
{
    public HealthSystem BrickHp {get; private set;}
    public float maxHp;

    public static System.Action OnAnyBrickDestroyed;
    void Awake()
    {
        BrickHp = new(maxHp);
    }
    void OnEnable()
    {
        BrickHp.OnDied += Die;
    }
    void OnDisable()
    {
        BrickHp.OnDied -= Die;
    }
    public void TakeDamage(float damage)
    {
        BrickHp.TakeDamage(damage);
    }

    private void Die()
    {
        Destroy(gameObject);
        OnAnyBrickDestroyed?.Invoke();
    }
}
