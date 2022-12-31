using System;
using System.Collections.Generic;
using AI;
using Grid;
using UnityEngine;
using Utils;

namespace Actions
{
    public class InteractAction : BaseAction
    {
        [SerializeField] private int _maxInteractDistance = 1;
        public override string ActionName => "Interact";

        private void Update()
        {
            if (!IsActive) return;
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            LevelGrid.Instance.GetDoorAtGridPosition(gridPosition).Interact(ActionComplete);
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();

            for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
            {
                for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                    Door door = LevelGrid.Instance.GetDoorAtGridPosition(testGridPosition);
                    if (door == null) continue;

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
