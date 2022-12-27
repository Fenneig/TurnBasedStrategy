using System;
using System.Collections.Generic;
using AI;
using Grid;
using Projectiles;
using UnityEngine;

namespace Actions
{
    public class GrenadeAction : BaseAction
    {
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
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > _maxThrowDistance) continue;
                    
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                    
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