using System;
using Grid;
using UnityEngine;

namespace Utils
{
    public class InteractSphere : MonoBehaviour, IInteractable
    {
        [SerializeField] private Material _greenMaterial;
        [SerializeField] private Material _redMaterial;
        [SerializeField] private MeshRenderer _renderer;

        private bool _isGreen;
        private Action _onInteractComplete;
        private float _timer;
        private bool _isActive;
        

        private void Start()
        {
            SetColorGreen();
            GridPosition gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(gridPosition, this);
        }

        private void Update()
        {
            if (!_isActive) return;
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                _isActive = false;
                _onInteractComplete();
            }
        }

        private void SetColorGreen()
        {
            _renderer.material = _greenMaterial;
            _isGreen = true;
        }
        private void SetColorRed()
        {
            _renderer.material = _redMaterial;
            _isGreen = false;
        }


        public void Interact(Action onInteractComplete)
        {
            _onInteractComplete = onInteractComplete;
            if (_isGreen) SetColorRed();
            else SetColorGreen();
            _timer = .5f;
            _isActive = true;
        }
    }
}