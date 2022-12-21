using Unity.Mathematics;
using UnityEngine;

namespace Utils
{
    public class BulletProjectile : MonoBehaviour
    {
        [SerializeField] private TrailRenderer _trailRenderer;
        [SerializeField] private Transform _bulletHitVfxPrefab;
        private Vector3 _targetPosition;
        public void Setup(Vector3 targetPosition)
        {
            _targetPosition = targetPosition;
        }

        private void Update()
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            float distanceBeforeMoving = Vector3.Distance(transform.position, _targetPosition);

            float moveSpeed = 200f;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            float distanceAfterMoving = Vector3.Distance(transform.position, _targetPosition);

            if (distanceBeforeMoving < distanceAfterMoving)
            {
                transform.position = _targetPosition;
                _trailRenderer.transform.parent = null;
                Instantiate(_bulletHitVfxPrefab, transform.position, quaternion.identity);
                Destroy(gameObject);
            }
        }
    }
}
