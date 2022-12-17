using System;
using System.Collections.Generic;
using Grid;
using UnitBased;
using UnityEngine;

namespace Actions
{
    public class ShootAction : BaseAction
    {
        private enum State
        {
            Aiming,
            Shooting,
            Cooloff
        }

        private int _maxShootDistance = 7;
        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;
        private bool _canShootBullet;
        private Vector3 _preShootForward;

        private void Update()
        {
            if (!IsActive) return;
            _stateTimer -= Time.deltaTime;
            Vector3 aimDirection = (_targetUnit.GetWorldPosition() - Unit.GetWorldPosition()).normalized;
            float rotateSpeed = 10f;

            switch (_state)
            {
                case State.Aiming:
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
                    transform.forward = Vector3.Lerp(transform.forward, _preShootForward, Time.deltaTime * rotateSpeed);
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
            _targetUnit.Damage();
        }

        public override string GetActionName()
        {
            return "Shoot";
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            ActionStart(onActionComplete);
            
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            _state = State.Aiming;
            float aimingStateTime = 1f;
            _stateTimer = aimingStateTime;
            _canShootBullet = true;
            _preShootForward = Unit.transform.forward;
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = Unit.GridPosition;

            for (int x = -_maxShootDistance; x <= _maxShootDistance; x++)
            {
                for (int z = -_maxShootDistance; z <= _maxShootDistance; z++)
                {
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxShootDistance) continue;

                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                    if (!LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;

                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                    if (targetUnit.IsEnemy == Unit.IsEnemy) continue;

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
    }
}