using System;
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
        
        
        [SerializeField] private int _width = 10;
        [SerializeField] private int _height = 10;
        [SerializeField] private float _cellSize = 2f;
        
        
        private GridSystemHex<GridObject> _gridSystemHex;
        
        
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

            _gridSystemHex = new GridSystemHex<GridObject>(_width, _height, _cellSize,
                (system, position) => new GridObject(system, position));
        }

        private void Start()
        {
            Pathfinding.Instance.Setup(_width, _height, _cellSize);
        }

        public void AddUnitAtGridPosition(GridPosition position, Unit unit) =>
            _gridSystemHex.GetGridObject(position).AddUnit(unit);

        public void RemoveUnitAtGridPosition(GridPosition position, Unit unit) =>
            _gridSystemHex.GetGridObject(position).RemoveUnit(unit);

        public GridPosition GetGridPosition(Vector3 worldPosition) =>
            _gridSystemHex.GetGridPosition(worldPosition);

        public Vector3 GetWorldPosition(GridPosition gridPosition) =>
            _gridSystemHex.GetWorldPosition(gridPosition);

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
            OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
        }

        public bool IsValidGridPosition(GridPosition gridPosition) =>
            _gridSystemHex.IsValidGridPosition(gridPosition);

        public bool HasAnyUnit(GridPosition gridPosition) =>
            _gridSystemHex.GetGridObject(gridPosition).HasAnyUnit();

        public Unit GetUnitAtGridPosition(GridPosition gridPosition) =>
            _gridSystemHex.GetGridObject(gridPosition).GetUnit();

        public IInteractable GetInteractableAtGridPosition(GridPosition gridPosition) =>
            _gridSystemHex.GetGridObject(gridPosition).Interactable;

        public void SetInteractableAtGridPosition(GridPosition gridPosition, IInteractable interactable) =>
            _gridSystemHex.GetGridObject(gridPosition).Interactable = interactable;

    }
}