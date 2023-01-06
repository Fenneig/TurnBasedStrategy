using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

namespace Grid
{
    public class GridSystemHex<TGridObject>
    {
        private const float HEX_VERTICAL_OFFSET_MULTIPLIER = .75f;
        private const float HEX_HORIZONTAL_ODD_OFFSET = .5f;
        private int _width;
        private int _height;
        private readonly float _cellSize;
        private TGridObject[,] _gridObjectArray;

        public int Width => _width;
        public int Height => _height;

        public GridSystemHex(int width, int height, float cellSize,
            Func<GridSystemHex<TGridObject>, GridPosition, TGridObject> createGridObject)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _gridObjectArray = new TGridObject[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    _gridObjectArray[x, z] = createGridObject(this, gridPosition);
                }
            }
        }

        public Vector3 GetWorldPosition(GridPosition gridPosition) =>
            (new Vector3(gridPosition.X, 0, gridPosition.Z * HEX_VERTICAL_OFFSET_MULTIPLIER) +
             (gridPosition.Z % 2 == 1 ? new Vector3(1, 0, 0) * HEX_HORIZONTAL_ODD_OFFSET : Vector3.zero)) * _cellSize;


        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            GridPosition roughXZ = new(
                Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize / HEX_VERTICAL_OFFSET_MULTIPLIER)
            );

            bool oddRow = roughXZ.Z % 2 == 1;

            List<GridPosition> neighbourGridPositionList = new List<GridPosition>
            {
                roughXZ + new GridPosition(+1, 0),
                roughXZ + new GridPosition(-1, 0),

                roughXZ + new GridPosition(0, +1),
                roughXZ + new GridPosition(0, -1),

                roughXZ + new GridPosition(oddRow ? +1 : -1, +1),
                roughXZ + new GridPosition(oddRow ? +1 : -1, -1)
            };

            GridPosition closestGridPosition = roughXZ;

            foreach (var neighbourGridPosition in neighbourGridPositionList)
            {
                if (Vector3.Distance(worldPosition, GetWorldPosition(neighbourGridPosition)) <
                    Vector3.Distance(worldPosition, GetWorldPosition(closestGridPosition)))
                    closestGridPosition = neighbourGridPosition;
            }

            return closestGridPosition;
        }

        public void CreateDebugObjects(Transform debugPrefab, Transform parentTransform)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);

                    Transform debugTransform =
                        GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), quaternion.identity,
                            parentTransform);

                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(_gridObjectArray[x, z]);
                }
            }
        }

        public TGridObject GetGridObject(GridPosition gridPosition) =>
            _gridObjectArray[gridPosition.X, gridPosition.Z];

        public bool IsValidGridPosition(GridPosition gridPosition) =>
            gridPosition.X >= 0 &&
            gridPosition.Z >= 0 &&
            gridPosition.X < _width &&
            gridPosition.Z < _height;
    }
}