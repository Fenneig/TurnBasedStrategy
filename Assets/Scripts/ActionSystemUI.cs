using System;
using System.Collections.Generic;
using UnitBased;
using UnityEngine;

public class ActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;

    private List<ActionButtonUI> _actionButtonUIList;

    private void Awake()
    {
        _actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += OnSelectedActionChanged;
        CreateUnitActionButtons();
    }

    private void OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        CreateUnitActionButtons();
    }

    private void OnSelectedActionChanged(object sender, EventArgs empty)
    {
        UpdateSelectedVisual();
    }

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
        {
            buttonUI.UpdateSelectedVisual();
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged -= OnSelectedActionChanged;
    }
}