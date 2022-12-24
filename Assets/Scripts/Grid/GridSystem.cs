using System;
using Unity.Mathematics;
using UnityEngine;

namespace Grid
{
    public class GridSystem<TGridObject>
    {
        private int _width;
        private int _height;
        private readonly float _cellSize;
        private TGridObject[,] _gridObjectArray;

        public int Width => _width;
        public int Height => _height;

        public GridSystem(int width, int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
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
            new Vector3(gridPosition.X, 0, gridPosition.Z) * _cellSize;

        public GridPosition GetGridPosition(Vector3 worldPosition)
        {
            return new GridPosition(Mathf.RoundToInt(worldPosition.x / _cellSize),
                Mathf.RoundToInt(worldPosition.z / _cellSize));
        }

        public void CreateDebugObjects(Transform debugPrefab, Transform parentTransform)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform debugTransform =
                        GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), quaternion.identity, parentTransform);
                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.SetGridObject(_gridObjectArray[x, z]);
                }
            }
        }

        public TGridObject GetGridObject(GridPosition gridPosition) => _gridObjectArray[gridPosition.X, gridPosition.Z];

        public bool IsValidGridPosition(GridPosition gridPosition) =>
            gridPosition.X >= 0 &&
            gridPosition.Z >= 0 &&
            gridPosition.X < _width &&
            gridPosition.Z < _height;
    }
}