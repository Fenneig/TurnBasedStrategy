using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pathfinder
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }

        public const int MOVE_STRAIGHT_COST = 10;

        [SerializeField] private Transform _gridDebugObjectPrefab;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private bool _createDebugObjects;
        private GridSystemHex<PathNode> _gridSystem;

        private void Awake()
        {
            Instance ??= this;
        }

        public void Setup(int width, int height, float cellSize)
        {
            _gridSystem = new GridSystemHex<PathNode>(width, height, cellSize, (system, position) =>
                new PathNode(position));

            if (_createDebugObjects) _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab, transform);

            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    Vector3 worldPosition = LevelGrid.Instance.GetWorldPosition(gridPosition);
                    float raycastOffsetDistance = 2f;
                    float maxRaycastDistance = 3f;
                    GetNode(x, z).IsWalkable = !Physics.Raycast(
                        worldPosition + Vector3.down * raycastOffsetDistance, Vector3.up,
                        maxRaycastDistance, _obstacleLayerMask);
                }
            }
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition,
            out int pathLength)
        {
            List<PathNode> openList = new List<PathNode>();
            List<PathNode> closeList = new List<PathNode>();

            PathNode startNode = _gridSystem.GetGridObject(startGridPosition);
            PathNode endNode = _gridSystem.GetGridObject(endGridPosition);
            openList.Add(startNode);
            for (int x = 0; x < _gridSystem.Width; x++)
            {
                for (int z = 0; z < _gridSystem.Height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = _gridSystem.GetGridObject(gridPosition);

                    pathNode.GCost = int.MaxValue;
                    pathNode.HCost = 0;
                    pathNode.CameFromPathNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateHeuristicDistance(startGridPosition, endGridPosition);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    pathLength = endNode.FCost;
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closeList.Contains(neighbourNode)) continue;

                    if (!neighbourNode.IsWalkable)
                    {
                        closeList.Add(neighbourNode);
                        continue;
                    }

                    int tentativeGCost = currentNode.GCost + MOVE_STRAIGHT_COST;

                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromPathNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateHeuristicDistance(neighbourNode.GridPosition, endGridPosition);

                        if (!openList.Contains(neighbourNode))
                        {
                            openList.Add(neighbourNode);
                        }
                    }
                }
            }

            //no path found
            pathLength = 0;
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            GridPosition currentNodeGridPosition = currentNode.GridPosition;

            bool oddRow = currentNodeGridPosition.Z % 2 == 1;
            for (int x = -1; x <= 1; x++)
            {
                for (int z = -1; z <= 1; z++)
                {
                    if (oddRow && x == -1 && z is -1 or +1) continue;
                    if (!oddRow && x == +1 && z is -1 or +1) continue;

                    if (!_gridSystem.IsValidGridPosition(currentNodeGridPosition + new GridPosition(x, z))) continue;
                    neighbourList.Add(GetNode(currentNodeGridPosition.X + x, currentNodeGridPosition.Z + z));
                }
            }

            return neighbourList;
        }

        private PathNode GetNode(int x, int z) =>
            _gridSystem.GetGridObject(new GridPosition(x, z));

        public int CalculateHeuristicDistance(GridPosition gridPositionA, GridPosition gridPositionB) =>
            Mathf.RoundToInt(MOVE_STRAIGHT_COST * Vector3.Distance(_gridSystem.GetWorldPosition(gridPositionA),
                _gridSystem.GetWorldPosition(gridPositionB)));

        private PathNode GetLowestFCostPathNode(List<PathNode> pathNodeList)
        {
            PathNode lowestFCostPathNode = pathNodeList[0];
            foreach (var pathNode in pathNodeList)
                if (pathNode.FCost < lowestFCostPathNode.FCost)
                    lowestFCostPathNode = pathNode;

            return lowestFCostPathNode;
        }

        private List<GridPosition> CalculatePath(PathNode endNode)
        {
            List<PathNode> pathNodeList = new List<PathNode>();
            pathNodeList.Add(endNode);
            PathNode currentNode = endNode;
            while (currentNode.CameFromPathNode != null)
            {
                pathNodeList.Add(currentNode.CameFromPathNode);
                currentNode = currentNode.CameFromPathNode;
            }

            pathNodeList.Reverse();
            List<GridPosition> gridPositionList = new List<GridPosition>();
            foreach (var pathNode in pathNodeList)
            {
                gridPositionList.Add(pathNode.GridPosition);
            }

            return gridPositionList;
        }

        public void SetIsWalkableGridPosition(GridPosition gridPosition, bool isWalkable) =>
            _gridSystem.GetGridObject(gridPosition).IsWalkable = isWalkable;

        public bool IsWalkableGridPosition(GridPosition gridPosition) =>
            _gridSystem.GetGridObject(gridPosition).IsWalkable;

        public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition) =>
            FindPath(startGridPosition, endGridPosition, out int pathLength) != null;

        public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            FindPath(startGridPosition, endGridPosition, out int pathLength);
            return pathLength;
        }
    }
}