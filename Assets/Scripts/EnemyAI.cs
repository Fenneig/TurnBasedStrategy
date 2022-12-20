using System;
using Actions;
using Grid;
using UnitBased;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State
    {
        WaitingForEnemyTurn,
        TakingTurn,
        Busy
    }

    private State _state;
    private float _timer;

    private void Awake()
    {
        _state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChanged += TurnSystem_OnTurnChanged;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn) return;

        switch (_state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                _timer -= Time.deltaTime;
                if (_timer <= 0f)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        _state = State.Busy;
                    }
                    else
                    {
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        _timer = .5f;
        _state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn) return;

        _state = State.TakingTurn;
        _timer = 2f;
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (var enemyUnit in UnitManager.Instance.EnemyUnitList)
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
                return true;

        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        SpinAction spinAction = enemyUnit.SpinAction;

        GridPosition actionGridPosition = enemyUnit.GridPosition;

        if (!spinAction.IsValidActionGridPosition(actionGridPosition)) return false;
        if (!enemyUnit.TrySpendActionPointsToTakeAction(spinAction)) return false;

        spinAction.TakeAction(actionGridPosition, onEnemyAIActionComplete);
        return true;
    }
}