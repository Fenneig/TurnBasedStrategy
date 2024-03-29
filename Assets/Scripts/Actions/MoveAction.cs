﻿using System;
using System.Collections.Generic;
using AI;
using Grid;
using Pathfinder;
using UnityEngine;

namespace Actions
{
    public class MoveAction : BaseAction
    {
        [SerializeField] private int _maxMoveDistance = 4;

        public event EventHandler OnStartMoving;
        public event EventHandler OnStopMoving;

        private List<Vector3> _positionList;
        private int _currentPositionIndex;

        public override string ActionName => "Move";

        private void Update()
        {
            if (!IsActive) return;

            Vector3 targetPosition = _positionList[_currentPositionIndex];

            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            float rotateSpeed = 10f;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

            float stoppingDistance = .1f;
            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                float moveSpeed = 4f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;
            }
            else
            {
                _currentPositionIndex++;
                if (_currentPositionIndex >= _positionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
            }
        }

        public override void TakeAction(GridPosition targetPosition, Action onActionComplete)
        {
            _currentPositionIndex = 0;

            List<GridPosition> pathGridPositionList =
                Pathfinding.Instance.FindPath(Unit.GridPosition, targetPosition, out int pathLength);
            _positionList = new List<Vector3>();

            foreach (var pathGridPosition in pathGridPositionList)
                _positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));

            OnStartMoving?.Invoke(this, EventArgs.Empty);

            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;


                    int pathfindingDistanceMultiplier = Pathfinding.MOVE_STRAIGHT_COST;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) ||
                        LevelGrid.Instance.HasAnyUnit(testGridPosition) ||
                        Unit.GridPosition == testGridPosition ||
                        !Pathfinding.Instance.IsWalkableGridPosition(testGridPosition) ||
                        !Pathfinding.Instance.HasPath(Unit.GridPosition, testGridPosition) ||
                        Pathfinding.Instance.GetPathLength(Unit.GridPosition, testGridPosition) > _maxMoveDistance * pathfindingDistanceMultiplier)
                        continue;

                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }

        public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
        {
            int targetCountAtPosition = Unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 10 * targetCountAtPosition
            };
        }
    }
}