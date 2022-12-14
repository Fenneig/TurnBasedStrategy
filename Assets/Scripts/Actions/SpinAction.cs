using System;
using System.Collections.Generic;
using AI;
using Grid;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;
        public override string ActionName => "Spin";
        private void Update()
        {
            if (!IsActive) return;

            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            _totalSpinAmount += spinAddAmount;

            if (_totalSpinAmount >= 360f)
                ActionComplete();
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            _totalSpinAmount = 0f;
            
            ActionStart(onActionComplete);
        }

        public override List<GridPosition> GetValidActionGridPositionList() =>
            new() {Unit.GridPosition};

        public override EnemyAIAction GetBestEnemyAIAction(GridPosition gridPosition)
        {
            return new EnemyAIAction
            {
                GridPosition = gridPosition,
                ActionValue = 0
            };
        }
    }
}