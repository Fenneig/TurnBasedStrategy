using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pathfinder
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }

        public const int MOVE_STRAIGHT_COST = 10;
        public const int MOVE_DIAGONAL_COST = 14;


        [SerializeField] private Transform _gridDebugObjectPrefab;
        [SerializeField] private LayerMask _obstacleLayerMask;
        [SerializeField] private bool _createDebugObjects;
        private GridSystemHex<PathNode> _gridSystemHex;

        private void Awake()
        {
            Instance ??= this;
        }

        public void Setup(int width, int height, float cellSize)
        {
            _gridSystemHex =
                new GridSystemHex<PathNode>(width, height, cellSize, (system, position) => new PathNode(position));

            if (_createDebugObjects) _gridSystemHex.CreateDebugObjects(_gridDebugObjectPrefab, transform);

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

            PathNode startNode = _gridSystemHex.GetGridObject(startGridPosition);
            PathNode endNode = _gridSystemHex.GetGridObject(endGridPosition);
            openList.Add(startNode);
            for (int x = 0; x < _gridSystemHex.Width; x++)
            {
                for (int z = 0; z < _gridSystemHex.Height; z++)
                {
                    GridPosition gridPosition = new GridPosition(x, z);
                    PathNode pathNode = _gridSystemHex.GetGridObject(gridPosition);

                    pathNode.GCost = int.MaxValue;
                    pathNode.HCost = 0;
                    pathNode.CameFromPathNode = null;
                }
            }

            startNode.GCost = 0;
            startNode.HCost = CalculateDistance(startGridPosition, endGridPosition);

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

                    int tentativeGCost = currentNode.GCost +
                                         CalculateDistance(currentNode.GridPosition, neighbourNode.GridPosition);

                    if (tentativeGCost < neighbourNode.GCost)
                    {
                        neighbourNode.CameFromPathNode = currentNode;
                        neighbourNode.GCost = tentativeGCost;
                        neighbourNode.HCost = CalculateDistance(neighbourNode.GridPosition, endGridPosition);

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

            GridPosition gridPosition = currentNode.GridPosition;

            for (int x = -1; x <= 1; x++)
            {
                if (gridPosition.X + x < 0 || gridPosition.X + x >= _gridSystemHex.Width) continue;
                for (int z = -1; z <= 1; z++)
                {
                    if (gridPosition.Z + z < 0 || gridPosition.Z + z >= _gridSystemHex.Height) continue;

                    neighbourList.Add(GetNode(gridPosition.X + x, gridPosition.Z + z));
                }
            }

            return neighbourList;
        }

        private PathNode GetNode(int x, int z)
        {
            return _gridSystemHex.GetGridObject(new GridPosition(x, z));
        }

        private int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
        {
            GridPosition gridPositionDistance = gridPositionA - gridPositionB;
            int distanceX = Mathf.Abs(gridPositionDistance.X);
            int distanceZ = Mathf.Abs(gridPositionDistance.Z);
            int remaining = Mathf.Abs(distanceX - distanceZ);
            return MOVE_DIAGONAL_COST * Mathf.Min(distanceX, distanceZ) + MOVE_STRAIGHT_COST * remaining;
        }

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
            _gridSystemHex.GetGridObject(gridPosition).IsWalkable = isWalkable;

        public bool IsWalkableGridPosition(GridPosition gridPosition) =>
            _gridSystemHex.GetGridObject(gridPosition).IsWalkable;

        public bool HasPath(GridPosition startGridPosition, GridPosition endGridPosition) =>
            FindPath(startGridPosition, endGridPosition, out int pathLength) != null;

        public int GetPathLength(GridPosition startGridPosition, GridPosition endGridPosition)
        {
            FindPath(startGridPosition, endGridPosition, out int pathLength);
            return pathLength;
        }
    }
}