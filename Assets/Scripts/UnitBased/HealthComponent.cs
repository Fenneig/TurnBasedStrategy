using System;
using UnityEngine;

namespace UnitBased
{
    public class HealthComponent : MonoBehaviour
    {
        [SerializeField] private int _health = 100;

        private int _maxHealth;
        public event EventHandler OnDead;
        public event EventHandler OnDamaged;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void Damage(int damageAmount)
        {
            _health -= damageAmount;
            OnDamaged?.Invoke(this,EventArgs.Empty);
            if (_health < 0)
                _health = 0;
            if (_health == 0)
            {
                OnDead?.Invoke(this, EventArgs.Empty);
            }
        }

        public float GetHealthNormalized()
        {
            return (float) _health / _maxHealth;
        }
    }
}
