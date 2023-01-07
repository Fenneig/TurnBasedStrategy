using System;
using System.Collections.Generic;
using AI;
using Grid;
using Pathfinder;
using Projectiles;
using UnityEngine;
using UnityEngine.UI;

namespace Actions
{
    public class GrenadeAction : BaseAction
    {
        private const float UNIT_SHOULDER_HEIGHT = 1.7f;

        [SerializeField] private Transform _grenadeProjectilePrefab;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private int _maxThrowDistance = 4;
        public override string ActionName => "Grenade";

        private void Update()
        {
            if (!IsActive) return;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            Transform grenadeProjectileTransform = Instantiate(_grenadeProjectilePrefab, Unit.WorldPosition, Quaternion.identity);
            grenadeProjectileTransform.GetComponent<GrenadeProjectile>().Setup(gridPosition, ActionComplete);
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxThrowDistance; x <= _maxThrowDistance; x++)
            {
                for (int z = -_maxThrowDistance; z <= _maxThrowDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                    if (!Pathfinding.Instance.HasPath(Unit.GridPosition,testGridPosition)) continue;
                    if (Pathfinding.Instance.GetPathLength(Unit.GridPosition, testGridPosition) > _maxThrowDistance * Pathfinding.MOVE_STRAIGHT_COST) continue;
                    var throwTargetWorldPosition = LevelGrid.Instance.GetWorldPosition(testGridPosition);
                    var throwDirection = (throwTargetWorldPosition - Unit.WorldPosition)
                        .normalized;
                    if (Physics.Raycast(Unit.WorldPosition + Vector3.up * UNIT_SHOULDER_HEIGHT,
                        throwDirection, Vector3.Distance(Unit.WorldPosition, throwTargetWorldPosition),
                        _obstacleLayerMask)) continue;
                    
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
                ActionValue = 0,
            };
        }
    }
}