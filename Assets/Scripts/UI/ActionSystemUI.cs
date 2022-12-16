using System;
using System.Collections.Generic;
using TMPro;
using UnitBased;
using UnityEngine;

namespace UI
{
    public class ActionSystemUI : MonoBehaviour
    {
        [SerializeField] private Transform _actionButtonPrefab;
        [SerializeField] private Transform _actionButtonContainerTransform;
        [SerializeField] private TextMeshProUGUI _actionPointText;

        private List<ActionButtonUI> _actionButtonUIList;

        private void Awake()
        {
            _actionButtonUIList = new List<ActionButtonUI>();
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
            UnitActionSystem.Instance.OnActionStart += OnActionStart;
            CreateUnitActionButtons();
            UpdateActionPoints();
        }

        private void OnSelectedUnitChanged(object sender, EventArgs empty)
        {
            CreateUnitActionButtons();
            UpdateActionPoints();
        }

        private void OnSelectedActionChanged(object sender, EventArgs empty)
        {
            UpdateSelectedVisual();
        }

        private void OnActionStart(object sender, EventArgs empty) => UpdateActionPoints();
        
        private void CreateUnitActionButtons()
        {
            _actionButtonUIList.Clear();

            foreach (Transform buttonTransform in _actionButtonContainerTransform)
            {
                Destroy(buttonTransform.gameObject);
            }

            Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

            foreach (var action in selectedUnit.BaseActions)
            {
                Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
                ActionButtonUI button = actionButtonTransform.GetComponent<ActionButtonUI>();
                button.SetBaseAction(action);
                _actionButtonUIList.Add(button);
            }

            UpdateSelectedVisual();
        }

        private void UpdateSelectedVisual()
        {
            foreach (var buttonUI in _actionButtonUIList)
                buttonUI.UpdateSelectedVisual();
        }

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
            UnitActionSystem.Instance.OnSelectedActionChanged -= OnSelectedActionChanged; 
            UnitActionSystem.Instance.OnActionStart -= OnActionStart;
        }

        private void UpdateActionPoints()
        {
            _actionPointText.text = $"Action points: {UnitActionSystem.Instance.SelectedUnit.ActionPoints}";
        }
    }
}