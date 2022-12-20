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
        public static event EventHandler OnAnyActionStarted;
        public static event EventHandler OnAnyActionCompleted;
        
        protected Unit ThisUnit;
        protected bool IsActive;
        protected Action OnActionComplete;

        public Unit Unit => ThisUnit;


        protected virtual void Awake()
        {
            ThisUnit = GetComponent<Unit>();
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

        protected void ActionStart(Action onActionComplete)
        {
            IsActive = true;
            OnActionComplete = onActionComplete;
            OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
        }
        
        protected void ActionComplete()
        {
            IsActive = false;
            OnActionComplete();
            OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        }


    }
}