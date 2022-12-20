using System;
using System.Collections.Generic;
using Actions;
using UnitBased;
using UnityEngine;

namespace Grid
{
    public class GridSystemVisual : MonoBehaviour
    {
        public static GridSystemVisual Instance { get; private set; }

        [Serializable]
        public struct GridVisualTypeMaterial
        {
            [SerializeField] private GridVisualType _gridVisualType;
            [SerializeField] private Material _material;
            public GridVisualType GridVisualType => _gridVisualType;
            public Material Material => _material;
        }

        public enum GridVisualType
        {
            White,
            Blue,
            Red,
            Yellow,
            RedSoft
        }

        [SerializeField] private Transform _gridSystemVisualSinglePrefab;
        [SerializeField] private List<GridVisualTypeMaterial> _gridVisualTypeMaterialList;

        private GridSystemVisualSingle[,] _gridSystemVisualSingleArray;


        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            _gridSystemVisualSingleArray =
                new GridSystemVisualSingle[LevelGrid.Instance.Widht, LevelGrid.Instance.Height];
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

            UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

            UpdateGridVisual();
        }

        private void HideAllGridPosition()
        {
            foreach (var gridSystemVisualSingle in _gridSystemVisualSingleArray)
                gridSystemVisualSingle.Hide();
        }

        private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
        {
            List<GridPosition> gridPositionList = new List<GridPosition>();
            for (int x = -range; x < range; x++)
            {
                for (int z = -range; z < range; z++)
                {
                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                    if (testDistance > range) continue;
                    GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) continue;

                    gridPositionList.Add(testGridPosition);
                }
            }

            ShowGridPositionList(gridPositionList, gridVisualType);
        }

        private void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
        {
            foreach (var gridPosition in gridPositionList)
                _gridSystemVisualSingleArray[gridPosition.X, gridPosition.Z]
                    .Show(GetGridVisualTypeMaterial(gridVisualType));
        }


        public void UpdateGridVisual()
        {
            HideAllGridPosition();
            Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;
            BaseAction selectedAction = UnitActionSystem.Instance.SelectedAction;
            GridVisualType gridVisualType;
            switch (selectedAction)
            {
                default:
                case MoveAction:
                    gridVisualType = GridVisualType.White;
                    break;
                case ShootAction shootAction:
                    ShowGridPositionRange(selectedUnit.GridPosition, shootAction.MaxShootDistance, GridVisualType.RedSoft);
                    gridVisualType = GridVisualType.Red;
                    break;
                case SpinAction:
                    gridVisualType = GridVisualType.Blue;
                    break;
            }

            ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        }

        private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) =>
            UpdateGridVisual();

        private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) =>
            UpdateGridVisual();

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedActionChanged -= UnitActionSystem_OnSelectedActionChanged;
            LevelGrid.Instance.OnAnyUnitMovedGridPosition -= LevelGrid_OnAnyUnitMovedGridPosition;
        }

        private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
        {
            foreach (var gridVisualTypeMaterial in _gridVisualTypeMaterialList)
            {
                if (gridVisualTypeMaterial.GridVisualType == gridVisualType)
                    return gridVisualTypeMaterial.Material;
            }

            Debug.LogError($"Could not find GridVisualTypeMaterial for GridVisualType {gridVisualType}");
            return null;
        }
    }
}