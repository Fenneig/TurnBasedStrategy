using System;
using Grid;
using Unity.VisualScripting;
using UnityEngine;

namespace UnitBased
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit _selectedUnit;
        [SerializeField] private LayerMask _unitLayerMask;

        private bool _isBusy;

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
            if (_isBusy) return;
            if (Input.GetMouseButtonDown((int) MouseButton.Left))
            {
                if (TryHandleUnitSelection()) return;

                GridPosition mouseGridPosition =
                    LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                if (_selectedUnit.MoveAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    SetBusy();
                    _selectedUnit.MoveAction.Move(mouseGridPosition, ClearBusy);
                }
            }

            if (Input.GetMouseButtonDown((int) MouseButton.Right))
            {
                SetBusy();
                _selectedUnit.SpinAction.Spin(ClearBusy);
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

        private void SetBusy() => _isBusy = true;


        private void ClearBusy() => _isBusy = false;
    }
}