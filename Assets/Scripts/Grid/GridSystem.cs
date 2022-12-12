using Unity.Mathematics;
using UnityEngine;

namespace Grid
{
    public class GridSystem
    {
        private int _width;
        private int _height;
        private readonly float _cellSize;
        private GridObject[,] _gridObjectArray;

        public GridSystem(int width, int height, float cellSize)
        {
            _width = width;
            _height = height;
            _cellSize = cellSize;
            _gridObjectArray = new GridObject[_width, _height];

            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    _gridObjectArray[x, z] = new GridObject(this, gridPosition);
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

        public void CreateDebugObjects(Transform debugPrefab)
        {
            for (int x = 0; x < _width; x++)
            {
                for (int z = 0; z < _height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Transform debugTransform =
                        GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), quaternion.identity);
                    GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                    gridDebugObject.GridObject = _gridObjectArray[x, z];
                }
            }
        }

        public GridObject GetGridObject(GridPosition gridPosition) => _gridObjectArray[gridPosition.X, gridPosition.Z];
    }
}