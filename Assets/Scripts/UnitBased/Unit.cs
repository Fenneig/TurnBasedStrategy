using Actions;
using Grid;
using UnityEngine;

namespace UnitBased
{
    public class Unit : MonoBehaviour
    {
        public GridPosition GridPosition { get; private set; }

        public MoveAction MoveAction { get; private set; }

        public SpinAction SpinAction { get; private set; }
        public BaseAction[] BaseActions { get; private set; }

        private void Awake()
        {
            MoveAction = GetComponent<MoveAction>();
            SpinAction = GetComponent<SpinAction>();
            BaseActions = GetComponents<BaseAction>();
        }

        private void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this);
        }

        private void Update()
        {
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition == GridPosition) return;
            LevelGrid.Instance.UnitMovedGridPosition(this, GridPosition, newGridPosition);
            GridPosition = newGridPosition;
        }
    }
}