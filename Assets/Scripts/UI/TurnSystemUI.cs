using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public class TurnSystemUI : MonoBehaviour
    {
        [SerializeField] private Button _endTurnButton;
        [SerializeField] private TextMeshProUGUI _turnNumberText;
        [SerializeField] private GameObject _enemyTurnVisual;

        private void Start()
        {
            _endTurnButton.onClick.AddListener(() => TurnSystem.Instance.NextTurn());
            TurnSystem.Instance.OnTurnChanged += TurnSystemUI_OnTurnSwitch;

            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndTurnButtonVisibility();
        }

        private void TurnSystemUI_OnTurnSwitch(object sender, EventArgs e) => NextTurn();

        private void NextTurn()
        {
            UpdateTurnText();
            UpdateEnemyTurnVisual();
            UpdateEndTurnButtonVisibility();
        }

        private void UpdateTurnText() =>
            _turnNumberText.text = $"TURN {TurnSystem.Instance.TurnNumber}";

        private void UpdateEnemyTurnVisual() =>
            _enemyTurnVisual.SetActive(!TurnSystem.Instance.IsPlayerTurn);

        private void UpdateEndTurnButtonVisibility() =>
            _endTurnButton.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn);

        private void OnDestroy()
        {
            TurnSystem.Instance.OnTurnChanged -= TurnSystemUI_OnTurnSwitch;
        }
    }
}