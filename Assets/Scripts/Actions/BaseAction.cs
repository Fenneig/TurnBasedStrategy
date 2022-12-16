using System;
using System.Collections.Generic;
using Grid;
using UnitBased;
using UnityEngine;

namespace Actions
{
    [RequireComponent(typeof(Unit))]
    public abstract class BaseAction : MonoBehaviour
    {
        protected Unit Unit;
        protected bool IsActive;
        protected Action OnActionComplete;

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }

        public abstract string GetActionName();

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition) =>
            GetValidActionGridPositionList().Contains(gridPosition);

        public abstract List<GridPosition> GetValidActionGridPositionList();

        public virtual int GetActionPointsCost()
        {
            return 1;
        }


    }
}