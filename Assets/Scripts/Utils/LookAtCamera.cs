using UnityEngine;

namespace Utils
{
    public class LookAtCamera : MonoBehaviour
    {
        private Transform _cameraTransform;

        private void Awake()
        {
            _cameraTransform = UnityEngine.Camera.main.transform;
        }

        private void LateUpdate()
        {
            transform.LookAt(_cameraTransform);
        }
    }
}