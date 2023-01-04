using System;
using Grid;
using Pathfinder;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    [RequireComponent(typeof(Animator))]
    public class Door : MonoBehaviour, IInteractable
    {
        [SerializeField] private bool _isOpen;
        [SerializeField] private UnityEvent _onOpenEvent;

        private Animator _animator;

        private GridPosition _gridPosition;
        private static readonly int IsOpen = Animator.StringToHash("IsOpen");
        private Action _onInteractComplete;
        private bool _isActive;
        private float _timer;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            _gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
            LevelGrid.Instance.SetInteractableAtGridPosition(_gridPosition, this);

            if (_isOpen) OpenDoor();
            else CloseDoor();

            UpdateDoor();
        }

        private void Update()
        {
            if (!_isActive) return;
            
            _timer -= Time.deltaTime;

            if (_timer <= 0f)
            {
                _isActive = false;
                _onInteractComplete();
            }
            
        }

        public void Interact(Action onInteractComplete)
        {
            _onInteractComplete = onInteractComplete;
            _isActive = true;
            _timer = .5f;
            if (_isOpen) CloseDoor();
            else OpenDoor();

            UpdateDoor();
        }

        private void OpenDoor()
        {
            _isOpen = true;
            _onOpenEvent?.Invoke();
        }


        private void CloseDoor() =>
            _isOpen = false;


        private void UpdateDoor()
        {
            Pathfinding.Instance.SetIsWalkableGridPosition(_gridPosition, _isOpen);
            _animator.SetBool(IsOpen, _isOpen);
        }
    }
}