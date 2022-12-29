using UnityEngine;

namespace UnitBased
{
    public class UnitRagdoll : MonoBehaviour
    {
        [SerializeField] private Transform _ragdollRootBone;

        public void Setup(Transform originalRootBone, Vector3 incomeDamagePosition)
        {
            MatchAllChildTransforms(originalRootBone, _ragdollRootBone);
            Vector3 damageImpulse = (incomeDamagePosition - transform.position).normalized;
            ApplyExplosionToRagdoll(_ragdollRootBone, 300f, transform.position + damageImpulse, 10f);
        }

        private void MatchAllChildTransforms(Transform root, Transform clone)
        {
            foreach (Transform child in root)
            {
                Transform cloneChild = clone.Find(child.name);
                if (cloneChild == null) continue;

                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MatchAllChildTransforms(child, cloneChild);
            }
        }

        private void ApplyExplosionToRagdoll(Transform root, float explosionForce, Vector3 explosionPosition,
            float explosionRange)
        {
            foreach (Transform child in root)
            {
                if (child.TryGetComponent(out Rigidbody childRigidbody))
                {
                    childRigidbody.AddExplosionForce(explosionForce, explosionPosition, explosionRange);
                }

                ApplyExplosionToRagdoll(child, explosionForce, explosionPosition, explosionRange);
            }
        }
    }
}