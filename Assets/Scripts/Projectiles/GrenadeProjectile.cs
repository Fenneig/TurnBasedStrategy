using System;
using Grid;
using Unity.Mathematics;
using UnityEngine;
using Utils;

namespace Projectiles
{
    public class GrenadeProjectile : MonoBehaviour
    {
        public static EventHandler OnAnyGrenadeExploded;

        [SerializeField] private AnimationCurve _arcYAnimationCurve;
        [SerializeField] private Transform _grenadeExplosionVFXPrefab;
        [SerializeField] private float _moveSpeed = 15f;
        [SerializeField] private float _damageRadius = 4f;
        [SerializeField] private int _damageAmount = 30;

        private Vector3 _targetPosition;
        private Action _onGrenadeBehaviourComplete;
        private float _totalDistance;
        private Vector3 _transformPositionXZ;
        
        private void Update()
        {
            Vector3 moveDirection = (_targetPosition - _transformPositionXZ).normalized;
            _transformPositionXZ += moveDirection * _moveSpeed * Time.deltaTime;

            float reachedTargetDistance = .2f;
            float distance = Vector3.Distance(_transformPositionXZ, _targetPosition);
            float distanceNormalized = _totalDistance == 0 ? 0 : 1 - distance / _totalDistance;

            float maxHeight = _totalDistance / 3f;
            float positionY = _arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
            transform.position = new Vector3(_transformPositionXZ.x, positionY, _transformPositionXZ.z);

            if (!(Vector3.Distance(_transformPositionXZ, _targetPosition) < reachedTargetDistance)) return;

            Collider[] colliderArray = new Collider[20];
            Physics.OverlapSphereNonAlloc(_targetPosition, _damageRadius, colliderArray);
                
            foreach (var collider in colliderArray)
            {
                if (collider == null) continue;
                
                if (collider.TryGetComponent(out IDamageable target))
                {
                    target.Damage(_damageAmount, transform.position);
                }
            }
            
            _onGrenadeBehaviourComplete?.Invoke();
                
            OnAnyGrenadeExploded?.Invoke(this, EventArgs.Empty);
                
            Instantiate(_grenadeExplosionVFXPrefab, _targetPosition, quaternion.identity);
                
            Destroy(gameObject);
        }

        public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
        {
            _onGrenadeBehaviourComplete = onGrenadeBehaviorComplete;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

            _transformPositionXZ = transform.position;
            _transformPositionXZ.y = 0;
            _totalDistance = Vector3.Distance(_transformPositionXZ, _targetPosition);
        }
    }
}
