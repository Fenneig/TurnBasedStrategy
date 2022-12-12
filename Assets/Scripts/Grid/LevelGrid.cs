using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class LevelGrid : MonoBehaviour
    {
        [SerializeField] private Transform _gridDebugObjectPrefab;
        private GridSystem _gridSystem;

        public static LevelGrid Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            _gridSystem = new GridSystem(10, 10, 2);
            _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
        }

        public void AddUnitAtGridPosition(GridPosition position, Unit unit) =>
            _gridSystem.GetGridObject(position).AddUnit(unit);


        public List<Unit> GetUnitListAtGridPosition(GridPosition position) =>
            _gridSystem.GetGridObject(position).GetUnitList();


        public void RemoveUnitAtGridPosition(GridPosition position, Unit unit) =>
            _gridSystem.GetGridObject(position).RemoveUnit(unit);


        public GridPosition GetGridPosition(Vector3 worldPosition) =>
            _gridSystem.GetGridPosition(worldPosition);

        public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
        {
            RemoveUnitAtGridPosition(fromGridPosition, unit);
            AddUnitAtGridPosition(toGridPosition, unit);
        }
    }
}