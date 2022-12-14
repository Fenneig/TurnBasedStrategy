using System;
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

        public void Spin(Action onActionComplete)
        {
            IsActive = true;
            _totalSpinAmount = 0f;
            OnActionComplete = onActionComplete;
        }
    }
}