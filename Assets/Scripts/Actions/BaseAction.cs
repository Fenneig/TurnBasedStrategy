using System;
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
    }
}