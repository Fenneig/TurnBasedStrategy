using Grid;

namespace Pathfinder
{
    public class PathNode
    {
        private GridPosition _gridPosition;
        private int _gCost;
        private int _hCost;
        private int _fCost;
        private PathNode _cameFromPathNode;
        
        public PathNode CameFromPathNode
        {
            get => _cameFromPathNode;
            set => _cameFromPathNode = value;
        }

        public GridPosition GridPosition => _gridPosition;

        public int GCost
        {
            get => _gCost;
            set => _gCost = value;
        }

        public int HCost
        {
            get => _hCost;
            set => _hCost = value;
        }

        public int FCost => _gCost + _hCost;

        public PathNode(GridPosition gridPosition)
        {
            _gridPosition = gridPosition;
        }

        public override string ToString()
        {
            return _gridPosition.ToString();
        }
    }
}