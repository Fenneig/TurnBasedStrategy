using System.Collections.Generic;
using UnitBased;
using Utils;

namespace Grid
{
    public class GridObject
    {
        private GridSystem<GridObject> _gridSystem;
        private GridPosition _gridPosition;
        private List<Unit> _units;

        public Door Door { get; set; }

        public GridObject(GridSystem<GridObject> gridSystem, GridPosition gridPosition)
        {
            _gridSystem = gridSystem;
            _gridPosition = gridPosition;
            _units = new List<Unit>();
        }

        public override string ToString()
        {
            string unitString = "";
            foreach (var unit in _units)
                unitString += unit + "\n";
            
            return _gridPosition.ToString() + $"\n{unitString}";
        }

        public void AddUnit(Unit unit) => _units.Add(unit);
        public void RemoveUnit(Unit unit) => _units.Remove(unit);
        public List<Unit> GetUnitList() => _units;

        public bool HasAnyUnit() => _units.Count > 0;

        public Unit GetUnit()
        {
            return HasAnyUnit() ? _units[0] : null;
        }
    }
}