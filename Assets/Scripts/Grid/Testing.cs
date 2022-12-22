using System.Collections.Generic;
using Pathfinder;
using UnitBased;
using UnityEngine;
using Utils;

namespace Grid
{
    public class Testing : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.T))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                GridPosition startGridPosition = UnitActionSystem.Instance.SelectedUnit.GridPosition;

                List<GridPosition> gridPositions = Pathfinding.Instance.FindPath(startGridPosition, mouseGridPosition);

                for (int i = 0; i < gridPositions.Count - 1; i++)
                {
                    Debug.DrawLine(LevelGrid.Instance.GetWorldPosition(gridPositions[i]),
                        LevelGrid.Instance.GetWorldPosition(gridPositions[i + 1]), Color.red, 10f);
                }
            }
        }
    }
}