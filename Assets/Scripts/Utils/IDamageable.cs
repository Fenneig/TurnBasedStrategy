using UnityEngine;

namespace Utils
{
    public interface IDamageable
    {
        public void Damage(int damageAmount, Vector3 incomeDamagePosition);
    }
}