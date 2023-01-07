using System;
using System.Collections.Generic;
using AI;
using Grid;
using Pathfinder;
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
            LevelGrid.Instance.GetInteractableAtGridPosition(gridPosition).Interact(ActionComplete);
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            bool oddRow = Unit.GridPosition.Z % 2 == 1;
            for (int x = -_maxInteractDistance; x <= _maxInteractDistance; x++)
            {
                for (int z = -_maxInteractDistance; z <= _maxInteractDistance; z++)
                {
                    //TODO убрать этот костыль, сейчас я не знаю как это сделать :(
                    if (_maxInteractDistance == 1)
                    {
                        if (oddRow && x == -1 && z is -1 or +1) continue;
                        if (!oddRow && x == +1 && z is -1 or +1) continue;
                    }

                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = Unit.GridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;
                    if (Pathfinding.Instance.GetPathLength(Unit.GridPosition, testGridPosition) > _maxInteractDistance * Pathfinding.MOVE_STRAIGHT_COST) continue;
                    IInteractable interactable = LevelGrid.Instance.GetInteractableAtGridPosition(testGridPosition);
                    if (interactable == null) continue;

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
