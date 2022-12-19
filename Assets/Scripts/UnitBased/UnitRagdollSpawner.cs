using System;
using UnityEngine;

namespace UnitBased
{
    public class UnitRagdollSpawner : MonoBehaviour
    {
        [SerializeField] private Transform _ragdollPrefab;
        [SerializeField] private Transform _originalRootBone;

        private HealthComponent _health;

        private void Awake()
        {
            _health = GetComponent<HealthComponent>();

            _health.OnDead += HealthComponent_OnDead;
        }

        private void HealthComponent_OnDead(object sender, EventArgs e)
        {
            Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
            UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
            unitRagdoll.Setup(_originalRootBone);
        }
    }
}
