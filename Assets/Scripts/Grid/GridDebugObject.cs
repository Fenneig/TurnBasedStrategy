using TMPro;
using UnityEngine;

namespace Grid
{
    public class GridDebugObject : MonoBehaviour
    {
        [SerializeField] private TextMeshPro _text;
        private GridObject _gridObject;
        
        public GridObject GridObject
        {
            set => _gridObject = value;
        }

        private void Update()
        {
            _text.text = _gridObject.ToString();
        }
    }
}