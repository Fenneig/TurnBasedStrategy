using System;
using Unity.VisualScripting;
using UnityEngine;

public class UnitActionSystem : MonoBehaviour
{
    [SerializeField] private Unit _selectedUnit;
    [SerializeField] private LayerMask _unitLayerMask;

    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;

    public Unit SelectedUnit
    {
        get => _selectedUnit;
        private set
        {
            _selectedUnit = value;
            OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
        } 
    }

    private void Awake()
    {
        Instance ??= this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown((int) MouseButton.Left))
        {
            if (!TryHandleUnitSelection())
                _selectedUnit.Move(MouseWorld.GetPosition());
        }
    }

    private bool TryHandleUnitSelection()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
        {
            if (hit.collider.gameObject.TryGetComponent(out Unit unit))
            {
                SelectedUnit = unit;
                return true;
            }
        }

        return false;
    }
}