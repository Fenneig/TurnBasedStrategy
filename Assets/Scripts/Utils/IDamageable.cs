using UnityEngine;

namespace Utils
{
    public interface IDamageable
    {
        void Damage(int damageAmount, Vector3 incomeDamagePosition);
    }
}