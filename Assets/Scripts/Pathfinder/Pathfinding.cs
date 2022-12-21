using Grid;
using UnityEngine;

namespace Pathfinder
{
    public class Pathfinding : MonoBehaviour
    {
        [SerializeField] private Transform _gridDebugObjectPrefab;
        private int _width;
        private int _height;
        private float _cellSize;
        private GridSystem<PathNode> _gridSystem;

        private void Awake()
        {
            _gridSystem = new GridSystem<PathNode>(10, 10, 2f, 
                (system, position) => new PathNode(position));
            _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
        }
    }
}