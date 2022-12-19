using System;
using TMPro;
using UnitBased;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UnitWorldUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _actionPointsText;
        [SerializeField] private Unit _unit;
        [SerializeField] private Image _healthBar;
        [SerializeField] private HealthComponent _healthComponent;

        private void Start()
        {
            Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
            _healthComponent.OnDamaged += HealthComponent_OnDamaged;
            UpdateActionPointsText();
            UpdateHealthBar();
        }

        private void UpdateActionPointsText()
        {
            _actionPointsText.text = _unit.ActionPoints.ToString();
        }

        private void UpdateHealthBar()
        {
            _healthBar.fillAmount = _healthComponent.GetHealthNormalized();
        }

        private void HealthComponent_OnDamaged(object sender, EventArgs e)
        {
            UpdateHealthBar();
        }

        private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e)
        {
            UpdateActionPointsText();
        }

        private void OnDestroy()
        {
            Unit.OnAnyActionPointsChanged -= Unit_OnAnyActionPointsChanged;
        }
    }
}