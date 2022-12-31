using System;
using System.Collections.Generic;
using Pathfinder;
using UnitBased;
using UnityEngine;
using Utils;

namespace Grid
{
    public class LevelGrid : MonoBehaviour
    {
        public static LevelGrid Instance { get; private set; }
        
        public event EventHandler OnAnyUnitMovedGridPosition;
        
        
        [SerializeField] private Transform _gridDebugObjectPrefab;
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private float _cellSize = 2f;
        
        
        private GridSystem<GridObject> _gridSystem;
        
        
        public int Width => _width;
        public int Height => _height;

        
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _gridSystem = new GridSystem<GridObject>(_width, _height, _cellSize,
                (system, position) => new GridObject(system, position));
        }

        private void Start()
        {
            Pathfinding.Instance.Setup(_width, _height, _cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition position, Unit unit) =>
            _gridSystem.GetGridObject(position).AddUnit(unit);


        public List<Unit> GetUnitListAtGridPosition(GridPosition position) =>
            _gridSystem.GetGridObject(position).GetUnitList();

        public void RemoveUnitAtGridPosition(GridPosition position, Unit unit)
        {
            _gridSystem.GetGridObject(position).RemoveUnit(unit);
        }

        public GridPosition GetGridPosition(Vector3 worldPosition) =>
            _gridSystem.GetGridPosition(worldPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition) =>
            _gridSystem.GetWorldPosition(gridPosition);

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
            OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
        }

        public bool IsValidGridPosition(GridPosition gridPosition) =>
            _gridSystem.IsValidGridPosition(gridPosition);

        public bool HasAnyUnit(GridPosition gridPosition) =>
            _gridSystem.GetGridObject(gridPosition).HasAnyUnit();

        public Unit GetUnitAtGridPosition(GridPosition gridPosition) =>
            _gridSystem.GetGridObject(gridPosition).GetUnit();

        public Door GetDoorAtGridPosition(GridPosition gridPosition) => 
            _gridSystem.GetGridObject(gridPosition).Door;
        
        public Door SetDoorAtGridPosition(GridPosition gridPosition, Door door) =>
            _gridSystem.GetGridObject(gridPosition).Door = door;

    }
}