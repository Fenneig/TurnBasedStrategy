using System;
using System.Collections.Generic;
using UnitBased;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        [SerializeField] private Transform _gridSystemVisualSinglePrefab;

        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;
        
        public static GridSystemVisual Instance { get; private set; }

        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            _gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.Widht, LevelGrid.Instance.Height];
            for (int x = 0; x < LevelGrid.Instance.Widht; x++)
            {
                for (int z = 0; z < LevelGrid.Instance.Height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(_gridSystemVisualSinglePrefab,
                            LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

                    _gridSystemVisualSingleArray[x, z] =
                        gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }
        
            HideAllGridPosition();
        }

        private void HideAllGridPosition()
        {
            foreach (var gridSystemVisualSingle in _gridSystemVisualSingleArray)
                gridSystemVisualSingle.Hide();
        }

        private void ShowGridPositionList(List<GridPosition> gridPositionList)
        {
            HideAllGridPosition();
            foreach (var gridPosition in gridPositionList)
                _gridSystemVisualSingleArray[gridPosition.X,gridPosition.Z].Show();
        }

        private void Update()
        {
            UpdateGridVisual();
        }

        public void UpdateGridVisual()
        {
            HideAllGridPosition();
            ShowGridPositionList(UnitActionSystem.Instance.SelectedUnit.MoveAction.GetValidActionGridPositionList());
        }
    }
}