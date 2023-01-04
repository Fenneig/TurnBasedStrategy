using Cinemachine;
using UnityEngine;

namespace Utils.Camera
{
    public class CameraInputController : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        private const float MinFollowYOffset = 1f;
        private const float MaxFollowYOffset = 12f;

        private CinemachineTransposer _cinemachineTransposer;
        private Vector3 _targetFollowOffset;

        private void Start()
        {
            _cinemachineTransposer = _camera.GetCinemachineComponent<CinemachineTransposer>();
            _targetFollowOffset = _cinemachineTransposer.m_FollowOffset;
        }
        
        private void Update()
        {
            HandleMovement();
            HandleRotation();
            HandleZoom();
        }

        private void HandleMovement()
        {
            Vector2 inputMoveDir = InputManager.Instance.GetCameraMoveVector();
            float moveSpeed = 10f;
            Vector3 moveVector = transform.forward * inputMoveDir.y + transform.right * inputMoveDir.x;
            transform.position += moveVector * moveSpeed * Time.deltaTime;
        }

        private void HandleRotation()
        {
            Vector3 rotateVector = new Vector3();
            rotateVector.y = InputManager.Instance.GetCameraRotateAmount();
            float rotateSpeed = 100f;
            transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
        }

        private void HandleZoom()
        {
            _targetFollowOffset.y += InputManager.Instance.GetCameraZoomAmount();
            float zoomSpeed = 5f;
            _targetFollowOffset.y = Mathf.Lerp(_targetFollowOffset.y, _targetFollowOffset.y, zoomSpeed * Time.deltaTime);
            _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowYOffset, MaxFollowYOffset);
            _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
                Time.deltaTime * zoomSpeed);
        }
    }
}