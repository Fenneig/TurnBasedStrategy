using System;
using Actions;
using Grid;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UnitBased
{
    public class UnitActionSystem : MonoBehaviour
    {
        [SerializeField] private Unit _selectedUnit;
        [SerializeField] private LayerMask _unitLayerMask;

        public BaseAction SelectedAction { get; set; }
        private bool _isBusy;

        public static UnitActionSystem Instance { get; private set; }
        public event EventHandler OnSelectedUnitChanged;

        public Unit SelectedUnit
        {
            get => _selectedUnit;
            private set
            {
                _selectedUnit = value;
                SelectedAction = _selectedUnit.MoveAction;
                OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Awake()
        {
            Instance ??= this;
        }

        private void Start()
        {
            SelectedUnit = _selectedUnit;
        }

        private void Update()
        {
            if (_isBusy) return;

            if (EventSystem.current.IsPointerOverGameObject()) return;

            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private bool TryHandleUnitSelection()
        {
            if (!Input.GetMouseButtonDown((int) MouseButton.Left)) return false;
            
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask))
            {
                if (hit.collider.gameObject.TryGetComponent(out Unit unit))
                {
                    if (SelectedUnit == unit) return false;
                    SelectedUnit = unit;
                    return true;
                }
            }

            return false;
        }

        private void HandleSelectedAction()
        {
            if (Input.GetMouseButtonDown((int) MouseButton.Left))
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
                if (SelectedAction.IsValidActionGridPosition(mouseGridPosition))
                {
                    SetBusy();
                    SelectedAction.TakeAction(mouseGridPosition, ClearBusy);
                }
            }
        }

        private void SetBusy() => _isBusy = true;


        private void ClearBusy() => _isBusy = false;
    }
}