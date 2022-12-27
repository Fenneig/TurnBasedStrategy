using System;
using Grid;
using UnitBased;
using UnityEngine;

namespace Projectiles
{
    public class GrenadeProjectile : MonoBehaviour
    {
        private Action _onGrenadeBehaviourComplete;
        
        [SerializeField] private float _moveSpeed = 15f;
        [SerializeField] private float _damageRadius = 4f;
        [SerializeField] private int _damageAmount = 30;

        private Vector3 _targetPosition;
        
        private void Update()
        {
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;
            transform.position += moveDirection * _moveSpeed * Time.deltaTime;

            float reachedTargetDistance = .2f;
            if (Vector3.Distance(transform.position, _targetPosition) < reachedTargetDistance)
            {
                Collider[] colliderArray = Physics.OverlapSphere(_targetPosition, _damageRadius);
                foreach (var collider in colliderArray)
                {
                    if (collider.TryGetComponent(out Unit targetUnit))
                    {
                        targetUnit.Damage(_damageAmount);
                    }
                }
                _onGrenadeBehaviourComplete?.Invoke();
                Destroy(gameObject);
            }
        }

        public void Setup(GridPosition targetGridPosition, Action onGrenadeBehaviorComplete)
        {
            _onGrenadeBehaviourComplete = onGrenadeBehaviorComplete;
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
        }
    }
}
