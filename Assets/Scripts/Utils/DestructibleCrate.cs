using System;
using Grid;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Utils
{
    public class DestructibleCrate : MonoBehaviour, IDamageable
    {
        public static event EventHandler OnAnyDestroyed;

        [SerializeField] private Transform[] _crateDestroyedPrefabs;

        public GridPosition GridPosition { get; private set; }

        private void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public void Damage(int _, Vector3 incomeDamagePosition)
        {
            OnAnyDestroyed?.Invoke(this, EventArgs.Empty);
            int randomPrefabIndex = Random.Range(0, _crateDestroyedPrefabs.Length);
            Transform crateDestroyedTransform = Instantiate(_crateDestroyedPrefabs[randomPrefabIndex],
                transform.position, quaternion.identity);
            Vector3 damageImpulse = (incomeDamagePosition - transform.position).normalized;
            ApplyExplosionToChildren(crateDestroyedTransform, 150f, transform.position + damageImpulse, 10f);
            Destroy(gameObject);
        }

        private void ApplyExplosionToChildren(Transform root, float explosionForce, Vector3 explosionPosition,
            float explosionRange)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                }

                ApplyExplosionToChildren(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}