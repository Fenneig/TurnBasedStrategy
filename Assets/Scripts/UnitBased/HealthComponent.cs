using System;
using UnityEngine;

namespace UnitBased
{
    public class HealthComponent : MonoBehaviour
    {
        public class OnDeadEventArgs : EventArgs
        {
            public Vector3 IncomeDamagePosition;
        }

        [SerializeField] private int _health = 100;

        private int _maxHealth;
        public event EventHandler<OnDeadEventArgs> OnDead;
        public event EventHandler OnDamaged;

        private void Awake()
        {
            _maxHealth = _health;
        }

        public void Damage(int damageAmount, Vector3 _incomeDamagePosition)
        {
            _health -= damageAmount;
            OnDamaged?.Invoke(this, EventArgs.Empty);
            if (_health < 0)
                _health = 0;
            if (_health == 0)
                OnDead?.Invoke(this, new OnDeadEventArgs
                {
                    IncomeDamagePosition = _incomeDamagePosition,
                });
        }

        public float GetHealthNormalized()
        {
            return (float) _health / _maxHealth;
        }
    }
}