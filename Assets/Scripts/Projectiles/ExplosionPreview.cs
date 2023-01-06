using UnityEngine;
using Utils;

namespace Projectiles
{
    public class ExplosionPreview : MonoBehaviour
    {
        [SerializeField] private GameObject _sphere;
        [SerializeField] private float _rotationSpeed;

        private void Update()
        {
            _sphere.transform.position = MouseWorld.GetPosition();
            _sphere.transform.Rotate(Vector3.one * Time.deltaTime * _rotationSpeed);
        }
    }
}
