using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
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
        Vector3 inputMoveDir = new Vector3(0, 0, 0);
        if (Input.GetKey(KeyCode.W)) inputMoveDir.z += 1f;
        if (Input.GetKey(KeyCode.A)) inputMoveDir.x -= 1f;
        if (Input.GetKey(KeyCode.S)) inputMoveDir.z -= 1f;
        if (Input.GetKey(KeyCode.D)) inputMoveDir.x += 1f;
        float moveSpeed = 10f;
        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * moveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotateVector = new Vector3();
        if (Input.GetKey(KeyCode.Q)) rotateVector.y += 1f;
        if (Input.GetKey(KeyCode.E)) rotateVector.y -= 1f;
        float rotateSpeed = 100f;
        transform.eulerAngles += rotateVector * rotateSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        float zoomAmount = 1f;
        if (Input.mouseScrollDelta.y > 0) _targetFollowOffset.y -= zoomAmount;
        if (Input.mouseScrollDelta.y < 0) _targetFollowOffset.y += zoomAmount;
        float zoomSpeed = 5f;
        _targetFollowOffset.y = Mathf.Lerp(_targetFollowOffset.y, _targetFollowOffset.y, zoomSpeed * Time.deltaTime);
        _targetFollowOffset.y = Mathf.Clamp(_targetFollowOffset.y, MinFollowYOffset, MaxFollowYOffset);
        _cinemachineTransposer.m_FollowOffset = Vector3.Lerp(_cinemachineTransposer.m_FollowOffset, _targetFollowOffset,
            Time.deltaTime * zoomSpeed);
    }
}