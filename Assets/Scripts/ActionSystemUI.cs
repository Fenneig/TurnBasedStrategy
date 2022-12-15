using System;
using UnitBased;
using UnityEngine;

public class ActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform _actionButtonPrefab;
    [SerializeField] private Transform _actionButtonContainerTransform;
    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
        CreateUnitActionButtons();
    }

    private void OnSelectedUnitChanged(object sender, EventArgs empty)
    {
        
        CreateUnitActionButtons();
    }

    private void CreateUnitActionButtons()
    {
        foreach (Transform buttonTransform in _actionButtonContainerTransform)
        {
            Destroy(buttonTransform.gameObject);
        }
        Unit selectedUnit = UnitActionSystem.Instance.SelectedUnit;

        foreach (var action in selectedUnit.BaseActions)
        {
          Transform actionButtonTransform = Instantiate(_actionButtonPrefab, _actionButtonContainerTransform);
          actionButtonTransform.GetComponent<ActionButtonUI>().SetBaseAction(action);
        }
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
    }
}
