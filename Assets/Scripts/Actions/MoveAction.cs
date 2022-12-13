using System.Collections.Generic;
using Grid;
using UnityEngine;
using Unit = UnitBased.Unit;

namespace Actions
{
    [RequireComponent(typeof(Unit))]
    public class MoveAction : MonoBehaviour
    {
        [SerializeField] private Animator _unitAnimator;
        [SerializeField] private int _maxMoveDistance = 4;

        private Vector3 _targetPosition;
        private Unit _unit;

        private static readonly int IsWalking = Animator.StringToHash("IsWalking");

        private void Awake()
        {
            _unit = GetComponent<Unit>();
            _targetPosition = transform.position;
        }

        private void Update()
        {
            float stoppingDistance = .1f;

            if (Vector3.Distance(transform.position, _targetPosition) > stoppingDistance)
            {
                Vector3 moveDirection = (_targetPosition - transform.position).normalized;

                float moveSpeed = 4f;
                transform.position += moveDirection * Time.deltaTime * moveSpeed;

                float rotateSpeed = 10f;
                transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

                _unitAnimator.SetBool(IsWalking, true);
            }
            else
            {
                _unitAnimator.SetBool(IsWalking, false);
            }
        }

        public void Move(GridPosition targetPosition) =>
            _targetPosition = LevelGrid.Instance.GetWorldPosition(targetPosition);

        public bool IsValidActionGridPosition(GridPosition gridPosition) =>
            GetValidActionGridPositionList().Contains(gridPosition);

        public List<GridPosition> GetValidActionGridPositionList()
        {
            List<GridPosition> validGridPositionList = new List<GridPosition>();
            GridPosition unitGridPosition = _unit.GridPosition;

            for (int x = -_maxMoveDistance; x <= _maxMoveDistance; x++)
            {
                for (int z = -_maxMoveDistance; z <= _maxMoveDistance; z++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;
                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition) ||
                        LevelGrid.Instance.HasAnyUnit(testGridPosition)) continue;
                    validGridPositionList.Add(testGridPosition);
                }
            }

            return validGridPositionList;
        }
    }
}