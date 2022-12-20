using System;
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Actions
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private int _maxMoveDistance = 4;

        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        private Vector3 _targetPosition;

        private static readonly int IsWalking = Animator.StringToHash("IsWalking");
        
        protected override void Awake()
        {
            base.Awake();
            _targetPosition = transform.position;
        }

        public override string GetActionName()
        {
            return "Move";
        }

        private void Update()
        {
            if (!IsActive) return;

            float stoppingDistance = .1f;
            Vector3 moveDirection = (_targetPosition - transform.position).normalized;

            if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
            }
            else
            {
                OnStopMoving?.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }

        public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
        {
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);

            OnStartMoving?.Invoke(this, EventArgs.Empty);

            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = ThisUnit.GridPosition;

            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) ||
                        LevelGrid.Instance.HasAnyUnit(testGridPosition)||
                        unitGridPosition == testGridPosition) continue;

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
    }
}