using UnityEngine;

namespace Utils
{
    public class MouseWorld : MonoBehaviour
    {
        [SerializeField] private LayerMask _mousePlaneLayerMask;

        private static MouseWorld _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static Vector3 GetPosition()
        {
            var ray = UnityEngine.Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out RaycastHit hit, float.MaxValue, _instance._mousePlaneLayerMask);
            return hit.point;
        }
    }
}