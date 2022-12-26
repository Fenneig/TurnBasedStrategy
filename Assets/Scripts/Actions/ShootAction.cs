using System;
using System.Collections.Generic;
using AI;
using Grid;
using UnitBased;
using UnityEngine;

namespace Actions
{
    public class ShootAction : BaseAction
    {
        public class OnShootEventArgs : EventArgs
        {
            public Unit TargetUnit;
            public Unit ShootingUnit;
        }

        private enum State
        {
            Aiming,
            Shooting,
            Cooloff
        }

        [SerializeField] private int _maxShootDistance = 7;
        [SerializeField] private int _damageAmount = 40;
        [SerializeField] private LayerMask _obstacleLayerMask;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShootBullet;

        public event EventHandler<OnShootEventArgs> OnShoot;

        public Unit TargetUnit => _targetUnit;
        public int MaxShootDistance => _maxShootDistance;

        private void Update()
        {
            if (!IsActive) return;
            _stateTimer -= Time.deltaTime;
            float rotateSpeed = 10f;

            switch (_state)
            {
                case State.Aiming:
                    Vector3 aimDirection = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                    break;
                case State.Shooting:
                    if (_canShootBullet)
                    {
                        Shoot();
                        _canShootBullet = false;
                    }

                    break;
                case State.Cooloff:
                    break;
            }

            if (_stateTimer <= 0f) NextState();
        }

        private void NextState()
        {
            switch (_state)
            {
                case State.Aiming:
                    _state = State.Shooting;
                    float shootingStateTime = .1f;
                    _stateTimer = shootingStateTime;
                    break;
                case State.Shooting:
                    _state = State.Cooloff;
                    float cooloffStateTime = .5f;
                    _stateTimer = cooloffStateTime;
                    break;
                case State.Cooloff:
                    ActionComplete();
                    break;
            }
        }

        private void Shoot()
        {
            OnShoot?.Invoke(this, new OnShootEventArgs
            {
                TargetUnit = _targetUnit,
                ShootingUnit = Unit
            });

            _targetUnit.Damage(_damageAmount);
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
            _canShootBullet = true;

            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            GridPosition unitGridPosition = Unit.GridPosition;

            return GetValidActionGridPositionList(unitGridPosition);
        }

        public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxShootDistance) continue;

                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) ||
                        !LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy == Unit.IsEnemy) continue;

                    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                    Vector3 shootDirection = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                    float unitShoulderHeight = 1.7f;

                    if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight,
                        shootDirection,
                        Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition()),
                        _obstacleLayerMask)) continue;

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
        {
            Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f)
            };
        }

        public int GetTargetCountAtPosition(GridPosition gridPosition)
        {
            return GetValidActionGridPositionList(gridPosition).Count;
        }
    }
}