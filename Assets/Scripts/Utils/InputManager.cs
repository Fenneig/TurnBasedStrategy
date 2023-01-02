#define USE_NEW_INPUT_SYSTEM
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utils
{
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance;
        private PlayerInputActions _playerInputActions;

        public Vector2 MouseScreenPosition
        {
            get
            {
#if USE_NEW_INPUT_SYSTEM
                return Mouse.current.position.ReadValue();
#else
                return Input.mousePosition;
#endif
            }
        }

        public bool IsMouseButtonDownThisFrame
        {
            get
            {
#if USE_NEW_INPUT_SYSTEM
                return _playerInputActions.Player.Click.WasPressedThisFrame();
#else
                return Input.GetMouseButtonDown((int) MouseButton.Left);
#endif
            }
        }

        private void Awake()
        {
            Instance ??= this;
            _playerInputActions = new PlayerInputActions();
            _playerInputActions.Player.Enable();
        }

        public Vector2 GetCameraMoveVector()
        {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.CameraMovement.ReadValue<Vector2>();
#else
            Vector2 inputMoveDir = new Vector2(0, 0);
            if (Input.GetKey(KeyCode.W)) inputMoveDir.y += 1f;
            if (Input.GetKey(KeyCode.A)) inputMoveDir.x -= 1f;
            if (Input.GetKey(KeyCode.S)) inputMoveDir.y -= 1f;
            if (Input.GetKey(KeyCode.D)) inputMoveDir.x += 1f;

            return inputMoveDir;
#endif
        }

        public float GetCameraZoomAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.CameraZoom.ReadValue<float>();
#else
            float zoomAmount = 0f;
            if (Input.mouseScrollDelta.y < 0) zoomAmount = +1f;
            if (Input.mouseScrollDelta.y > 0) zoomAmount = -1f;
            return zoomAmount;
#endif
        }

        public float GetCameraRotateAmount()
        {
#if USE_NEW_INPUT_SYSTEM
            return _playerInputActions.Player.CameraRotate.ReadValue<float>();
#else
            float rotateAmount = 0f;
            if (Input.GetKey(KeyCode.Q)) rotateAmount = +1f;
            if (Input.GetKey(KeyCode.E)) rotateAmount = -1f;
            return rotateAmount;
#endif
        }
    }
}