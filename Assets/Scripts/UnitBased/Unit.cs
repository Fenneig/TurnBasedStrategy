using System;
using Actions;
using Grid;
using UnityEngine;
using Utils;

namespace UnitBased
{
    public class Unit : MonoBehaviour
    {
        [SerializeField] private int _maxActionPoints = 2;
        [SerializeField] private bool _isEnemy;

        public static event EventHandler OnAnyActionPointsChanged;
        public static event EventHandler OnAnyUnitSpawned;
        public static event EventHandler OnAnyUnitDead;

        public HealthComponent Health { get; private set; }
        public GridPosition GridPosition { get; private set; }
        public BaseAction[] BaseActions { get; private set; }
        public int ActionPoints { get; set; }
        public bool IsEnemy => _isEnemy;
        public Vector3 WorldPosition => transform.position;

        private void Awake()
        {
            BaseActions = GetComponents<BaseAction>();
            Health = GetComponent<HealthComponent>();
            ActionPoints = _maxActionPoints;
        }

        public T GetAction<T>() where T : BaseAction
        {
            foreach (var baseAction in BaseActions)
            {
                if (baseAction is T) return (T)baseAction;
            }

            return null;
        }
        

        private void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.AddUnitAtGridPosition(GridPosition, this);
            TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
            Health.OnDead += HealthComponent_OnDead;
            OnAnyUnitSpawned?.Invoke(this,EventArgs.Empty);
        }

        private GridPosition _newGridPosition;
        private GridPosition _oldGridPosition;

        private void Update()
        {
            _newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            if (_newGridPosition == GridPosition) return;
            _oldGridPosition = GridPosition;
            GridPosition = _newGridPosition;
            
            LevelGrid.Instance.UnitMovedGridPosition(this, _oldGridPosition, _newGridPosition);
        }

        public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
        {
            if (!CanSpendActionPointToTakeAction(baseAction)) return false;
            
            SpendActionPoints(baseAction.ActionPointsCost);
            return true;
        }

        public bool CanSpendActionPointToTakeAction(BaseAction baseAction) =>
            ActionPoints >= baseAction.ActionPointsCost;


        private void SpendActionPoints(int amount)
        {
            ActionPoints -= amount;

            OnAnyActionPointsChanged?.Invoke(this,EventArgs.Empty);
        }

        private void HealthComponent_OnDead(object sender, EventArgs args)
        {
            LevelGrid.Instance.RemoveUnitAtGridPosition(GridPosition, this);
            
            OnAnyUnitDead?.Invoke(this,EventArgs.Empty);
            
            Destroy(gameObject);
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

        public void Damage(int damageAmount) =>
            Health.Damage(damageAmount);

        public float GetHealthNormalized() =>
            Health.GetHealthNormalized();
    }
}