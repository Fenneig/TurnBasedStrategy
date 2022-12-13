using System;
using UnityEngine;

namespace UnitBased
{
    public class UnitSelectedVisual : MonoBehaviour
    {
        [SerializeField] private Unit _unit;

        private MeshRenderer _meshRenderer;

        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        private void Start()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged += OnSelectedUnitChanged;
            UpdateVisual();
        }

        private void OnSelectedUnitChanged(object sender, EventArgs empty) => UpdateVisual();

        private void UpdateVisual() =>
            _meshRenderer.enabled = UnitActionSystem.Instance.SelectedUnit == _unit;

        private void OnDestroy()
        {
            UnitActionSystem.Instance.OnSelectedUnitChanged -= OnSelectedUnitChanged;
        }
    }
}