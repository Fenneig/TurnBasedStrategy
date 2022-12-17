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

        private BaseAction _selectedAction;

        public BaseAction SelectedAction
        {
            get => _selectedAction;
            set
            {
                _selectedAction = value;
                OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
            }
        }

        private bool _isBusy;
        private bool IsBusy
        {
            set
            {
                _isBusy = value;
                OnBusyChanged?.Invoke(this, _isBusy);
            }
        }
        
        

        public static UnitActionSystem Instance { get; private set; }
        public event EventHandler OnSelectedUnitChanged;
        public event EventHandler OnSelectedActionChanged;
        public event EventHandler OnActionStart;
        public event EventHandler<bool> OnBusyChanged;

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
            if (!TurnSystem.Instance.IsPlayerTurn) return;
            if (EventSystem.current.IsPointerOverGameObject()) return;
            if (TryHandleUnitSelection()) return;

            HandleSelectedAction();
        }

        private bool TryHandleUnitSelection()
        {
            if (!Input.GetMouseButtonDown((int) MouseButton.Left)) return false;
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _unitLayerMask)) return false;
            if (!hit.collider.gameObject.TryGetComponent(out Unit unit)) return false;
            if (unit.IsEnemy) return false; 
            if (SelectedUnit == unit) return false;

            SelectedUnit = unit;
            return true;
        } 

        private void HandleSelectedAction()
        {
            if (!Input.GetMouseButtonDown((int) MouseButton.Left)) return;
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            if (!SelectedAction.IsValidActionGridPosition(mouseGridPosition)) return;
            if (!SelectedUnit.TrySpendActionPointsToTakeAction(SelectedAction)) return;
            IsBusy = true;
            SelectedAction.TakeAction(mouseGridPosition, () => IsBusy = false); 
            OnActionStart?.Invoke(this, EventArgs.Empty);
        }
    }
}