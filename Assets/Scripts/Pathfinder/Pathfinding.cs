using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Pathfinder
{
    public class Pathfinding : MonoBehaviour
    {
        public static Pathfinding Instance { get; private set; }
        
        private const int MOVE_STRAIGHT_COST = 10;
        private const int MOVE_DIAGONAL_COST = 14;

        [SerializeField] private Transform _gridDebugObjectPrefab;
        private int _width;
        private int _height;
        private float _cellSize;
        private GridSystem<PathNode> _gridSystem;

        private void Awake()
        {
            Instance ??= this;
            _gridSystem = new GridSystem<PathNode>(10, 10, 2f,
                (system, position) => new PathNode(position));
            _gridSystem.CreateDebugObjects(_gridDebugObjectPrefab);
        }

        public List<GridPosition> FindPath(GridPosition startGridPosition, GridPosition endGridPosition)
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
            startNode.HCost = CalculateDistance(startGridPosition, endGridPosition);

            while (openList.Count > 0)
            {
                PathNode currentNode = GetLowestFCostPathNode(openList);

                if (currentNode == endNode)
                {
                    return CalculatePath(endNode);
                }

                openList.Remove(currentNode);
                closeList.Add(currentNode);

                foreach (PathNode neighbourNode in GetNeighbourList(currentNode))
                {
                    if (closeList.Contains(neighbourNode)) continue;

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
            return null;
        }

        private List<PathNode> GetNeighbourList(PathNode currentNode)
        {
            List<PathNode> neighbourList = new List<PathNode>();

            GridPosition gridPosition = currentNode.GridPosition;

            for (int x = -1; x <= 1; x++)
            {
                if (gridPosition.X + x < 0 || gridPosition.X + x >= _gridSystem.Width) continue;
                for (int z = -1; z <= 1; z++)
                {
                    if (gridPosition.Z + z < 0 || gridPosition.Z + z >= _gridSystem.Height) continue;

                    neighbourList.Add(GetNode(gridPosition.X + x, gridPosition.Z + z));
                }
            }

            return neighbourList;
        }

        private PathNode GetNode(int x, int z)
        {
            return _gridSystem.GetGridObject(new GridPosition(x, z));
        }

        public int CalculateDistance(GridPosition gridPositionA, GridPosition gridPositionB)
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
    }
}