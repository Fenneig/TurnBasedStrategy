﻿using System;
using System.Collections.Generic;
using Grid;
using UnitBased;
using UnityEngine;

namespace Actions
{
    public class SpinAction : BaseAction
    {
        private float _totalSpinAmount;

        private void Update()
        {
            if (!IsActive) return;

            float spinAddAmount = 360f * Time.deltaTime;
            transform.eulerAngles += new Vector3(0, spinAddAmount, 0);
            _totalSpinAmount += spinAddAmount;

            if (_totalSpinAmount >= 360f)
            {
                IsActive = false;
                OnActionComplete?.Invoke();
            }
        }

        public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
        {
            IsActive = true;
            _totalSpinAmount = 0f;
            OnActionComplete = onActionComplete;
        }

        public override List<GridPosition> GetValidActionGridPositionList() =>
            new() {Unit.GridPosition};

        public override string GetActionName()
        {
            return "Spin";
        }

        public override int GetActionPointsCost()
        {
            return 2;
        }
    }
}