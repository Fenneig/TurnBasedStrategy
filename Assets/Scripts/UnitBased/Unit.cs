using Actions;
using Grid;
using UnityEngine;

namespace UnitBased
{
    [RequireComponent(typeof(MoveAction))]
    public class Unit : MonoBehaviour
    {
        private MoveAction _moveAction;
        private GridPosition _gridPosition;

        public GridPosition GridPosition => _gridPosition;

        public MoveAction MoveAction => _moveAction;

        private void Awake()
        {
            _moveAction = GetComponent<MoveAction>();
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(_gridPosition, this);
        }

        private void Update()
        {
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition == _gridPosition) return;
            LevelGrid.Instance.UnitMovedGridPosition(this, _gridPosition, newGridPosition);
            _gridPosition = newGridPosition;
        }
    }
}