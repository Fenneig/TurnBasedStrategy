using System;
using Actions;
using UnitBased;
using UnityEngine;
using Utils;

namespace AI
{
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
            EnemyAIAction bestEnemyAIAction = null;
            BaseAction bestBaseAction = null;
            foreach (BaseAction baseAction in enemyUnit.BaseActions)
            {
                if (!enemyUnit.CanSpendActionPointToTakeAction(baseAction)) 
                    continue;

                if (bestEnemyAIAction == null)
                {
                    bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    bestBaseAction = baseAction;
                }
                else
                {
                    EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
                    if (testEnemyAIAction != null && testEnemyAIAction.ActionValue > bestEnemyAIAction.ActionValue)
                    {
                        bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                        bestBaseAction = baseAction;
                    }
                }
            }

            if (bestEnemyAIAction == null || !enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction)) return false;
        
            bestBaseAction.TakeAction(bestEnemyAIAction.GridPosition ,onEnemyAIActionComplete);
            return true;

        }
    }
}