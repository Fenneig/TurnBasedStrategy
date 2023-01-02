using UnityEngine;

namespace Utils
{
    public class MouseWorld : MonoBehaviour
    {
        [SerializeField] private LayerMask _mousePlaneLayerMask;

        private static MouseWorld _instance;
        private static UnityEngine.Camera _mainCamera;

        private void Awake()
        {
            _instance = this;
            _mainCamera = UnityEngine.Camera.main;
        }

        public static Vector3 GetPosition()
        {
            var ray = _mainCamera.ScreenPointToRay(InputManager.Instance.MouseScreenPosition);
            Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _instance._mousePlaneLayerMask);
            return hit.point;
        }
    }
}