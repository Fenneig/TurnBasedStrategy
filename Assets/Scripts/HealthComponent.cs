using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    [SerializeField] private int _health = 100;

    public event EventHandler OnDead;

    public void Damage(int damageAmount)
    {
        _health -= damageAmount;
        if (_health < 0)
            _health = 0;
        if (_health == 0)
        {
            OnDead?.Invoke(this, EventArgs.Empty);
        }
    }
}
