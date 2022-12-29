using System;
using UnityEngine;
using Utils;

namespace Pathfinder
{
    public class PathfinderUpdater : MonoBehaviour
    {
        private void Start()
        {
            DestructibleCrate.OnAnyDestroyed += DestructibleCrate_OnAnyDestroyed;
        }

        private void DestructibleCrate_OnAnyDestroyed(object sender, EventArgs e)
        {
            DestructibleCrate destructibleCrate = sender as DestructibleCrate;

            if (destructibleCrate != null)
                Pathfinding.Instance.SetIsWalkableGridPosition(destructibleCrate.GridPosition, true);
        }

        private void OnDestroy()
        {
            DestructibleCrate.OnAnyDestroyed -= DestructibleCrate_OnAnyDestroyed;
        }
    }
}