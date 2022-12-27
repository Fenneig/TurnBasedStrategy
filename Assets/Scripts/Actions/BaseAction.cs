using System;
using System.Collections.Generic;
using AI;
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

        [SerializeField] private int _actionPointsCost = 1;
        public int ActionPointsCost => _actionPointsCost;
        public abstract string ActionName { get; }

        protected bool IsActive;
        protected Action OnActionComplete;

        public Unit Unit { get; protected set; }

        protected virtual void Awake()
        {
            Unit = GetComponent<Unit>();
        }

        public abstract void TakeAction(GridPosition gridPosition, Action onActionComplete);

        public virtual bool IsValidActionGridPosition(GridPosition gridPosition) =>
            GetValidActionGridPositionList().Contains(gridPosition);

        public abstract List<GridPosition> GetValidActionGridPositionList();

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

        public EnemyAIAction GetBestEnemyAIAction()
        {
            List<EnemyAIAction> enemyAIActionList = new List<EnemyAIAction>();

            List<GridPosition> validActionGridPositionList = GetValidActionGridPositionList();

            foreach (GridPosition gridPosition in validActionGridPositionList)
            {
                EnemyAIAction enemyAIAction = GetBestEnemyAIAction(gridPosition);
                enemyAIActionList.Add(enemyAIAction);
            }

            if (enemyAIActionList.Count == 0) return null;
            
            enemyAIActionList.Sort((AIactionA, AIactionB) =>
                AIactionB.ActionValue - AIactionA.ActionValue);
            return enemyAIActionList[0];
        }

        public abstract EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition);
    }
}