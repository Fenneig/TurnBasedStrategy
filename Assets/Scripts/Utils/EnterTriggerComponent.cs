using System;
using UnityEngine;
using UnityEngine.Events;

namespace Utils
{
    public class EnterTriggerComponent : MonoBehaviour
    {
        [SerializeField] private string[] _tags;
        [SerializeField] private LayerMask _unitLayer;
        [SerializeField] private EnterEvent _action;

        private void OnTriggerEnter(Collider other)
        {
            if (_tags.Length == 0)
            {
                if (other.gameObject.layer == _unitLayer)
                {
                    _action?.Invoke(other.gameObject);
                }
            }
            else
            {
                foreach (var tag in _tags)
                {
                    if (other.gameObject.CompareTag(tag))
                    {
                        _action?.Invoke(other.gameObject);
                    }
                }
            }
            
        }


        [Serializable]
        public class EnterEvent : UnityEvent<GameObject>
        {
        }
    }
}