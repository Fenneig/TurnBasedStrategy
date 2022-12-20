using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnitBased
{
    public class UnitManager : MonoBehaviour
    {
        public static UnitManager Instance { get; private set; }
        
        private List<Unit> _unitList;
        private List<Unit> _friendlyUnitList;
        private List<Unit> _enemyUnitList;

        public List<Unit> UnitList => _unitList;
        public List<Unit> FriendlyUnitList => _friendlyUnitList;
        public List<Unit> EnemyUnitList => _enemyUnitList;

        private void Awake()
        {
            Instance ??= this;
            
            _unitList = new List<Unit>();
            _friendlyUnitList = new List<Unit>();
            _enemyUnitList = new List<Unit>();
        }

        private void Start()
        {
            Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
            Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        }

        private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            if (unit == null)
            {
                Debug.LogError("There is no unit!!");
                return;
            }
            _unitList.Add(unit);
            if (unit.IsEnemy) _enemyUnitList.Add(unit);
            else _friendlyUnitList.Add(unit);
        }

        private void Unit_OnAnyUnitDead(object sender, EventArgs e)
        {
            Unit unit = sender as Unit;
            if (unit == null)
            {
                Debug.LogError("There is no unit!!");
                return;
            }
            _unitList.Remove(unit);
            if (unit.IsEnemy) _enemyUnitList.Remove(unit);
            else _friendlyUnitList.Remove(unit);
        }

        private void OnDestroy()
        {
            Unit.OnAnyUnitSpawned -= Unit_OnAnyUnitSpawned;
            Unit.OnAnyUnitDead -= Unit_OnAnyUnitDead;
        }
    }
}