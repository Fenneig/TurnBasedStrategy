using System;
using System.Collections.Generic;
using AI;
using Grid;
using UnitBased;
using UnityEngine;

namespace Actions
{
    public class SwordAction : BaseAction
    {
        private enum State
        {
            SwingingSwordBeforeHit,
            SwingingSwordAfterHit,
        }

        public static event EventHandler OnAnySwordHit;

        public event EventHandler OnSwordActionStarted;
        public event EventHandler OnSwordActionCompleted;

        [SerializeField] private int _damageAmount = 100;
        [SerializeField] private int _maxSwordDistance = 1;
        public override string ActionName => "Sword";
        public int MaxSwordDistance => _maxSwordDistance;

        private State _state;
        private float _stateTimer;
        private Unit _targetUnit;


        private void Update()
        {
            if (!IsActive) return;
            
            _stateTimer -= Time.deltaTime;
            switch (_state)
            {
                case State.SwingingSwordBeforeHit:
                    Vector3 aimDirection = (_targetUnit.WorldPosition - Unit.WorldPosition).normalized;
                    float rotateSpeed = 10f;
                    transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * rotateSpeed);
                    break;
                case State.SwingingSwordAfterHit:
                    break;
            }

            if (_stateTimer <= 0f) NextState();
        }

        private void NextState()
        {
            switch (_state)
            {
                case State.SwingingSwordBeforeHit:
                    _state = State.SwingingSwordAfterHit;
                    float afterHitStateTime = .5f;
                    _stateTimer = afterHitStateTime;
                    float unitShoulderHeight = 1.7f;
                    _targetUnit.Damage(_damageAmount, transform.position + Vector3.up * unitShoulderHeight);
                    OnAnySwordHit?.Invoke(this,EventArgs.Empty);
                    break;
                case State.SwingingSwordAfterHit:
                    OnSwordActionCompleted?.Invoke(this,EventArgs.Empty);
                    ActionComplete();
                    break;
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            
            _state = State.SwingingSwordBeforeHit;
            float beforeHitStateTime = .75f;
            _stateTimer = beforeHitStateTime;
            
            OnSwordActionStarted?.Invoke(this, EventArgs.Empty);
            
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxSwordDistance; x <= _maxSwordDistance; x++)
            {
                for (int z = -_maxSwordDistance; z <= _maxSwordDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                    if (!LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;
                    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition); 
                    if (targetUnit.IsEnemy == Unit.IsEnemy) continue;
                    
                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 200
            };
        }
    }
}
