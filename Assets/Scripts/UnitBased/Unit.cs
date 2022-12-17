using System;
using Actions;
using Grid;
using UnityEngine;

namespace UnitBased
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int _maxActionPoints = 2;
        [SerializeField] private bool _isEnemy;

        public static event EventHandler OnAnyActionPointsChanged;
        
        public GridPosition GridPosition { get; private set; }
        public MoveAction MoveAction { get; private set; }
        public SpinAction SpinAction { get; private set; }
        public BaseAction[] BaseActions { get; private set; }
        public int ActionPoints { get; set; }

        public bool IsEnemy => _isEnemy;

        private void Awake()
        {
            MoveAction = GetComponent<MoveAction>();
            SpinAction = GetComponent<SpinAction>();
            BaseActions = GetComponents<BaseAction>();
            ActionPoints = 2;
        }

        private void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this);
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
        }

        private void Update()
        {
            var newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (newGridPosition == GridPosition) return;
            LevelGrid.Instance.UnitMovedGridPosition(this, GridPosition, newGridPosition);
            GridPosition = newGridPosition;
        }

        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (CanSpendActionPointToTakeAction(baseAction))
            {
                SpendActionPoints(baseAction.GetActionPointsCost());
                return true;
            }

            return false;
        }

        private bool CanSpendActionPointToTakeAction(BaseAction baseAction) =>
            ActionPoints >= baseAction.GetActionPointsCost();


        private void SpendActionPoints(int amount)
        {
            ActionPoints -= amount;

            OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
        }

        private void TurnSystem_OnTurnChanged(object sender, EventArgs args)
        {
            if (IsEnemy && !TurnSystem.Instance.IsPlayerTurn ||
                !IsEnemy && TurnSystem.Instance.IsPlayerTurn)
            {
                ActionPoints = _maxActionPoints;

                OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void OnDestroy()
        {
            TurnSystem.Instance.OnTurnChanged -= TurnSystem_OnTurnChanged;
        }

        public void Damage()
        {
            Debug.Log(transform + " damaged");
        }

        public Vector3 GetWorldPosition() => 
            transform.position;
    }
}