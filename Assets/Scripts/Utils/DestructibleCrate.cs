using System;
using Grid;
using UnityEngine;

namespace Utils
{
    public class DestructibleCrate : MonoBehaviour, IDamageable
    {
        public static event EventHandler OnAnyDestroyed;

        public GridPosition GridPosition { get; private set; }

        private void Start()
        {
            GridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        }

        public void Damage(int _, Vector3 incomeDamagePosition)
        {
            OnAnyDestroyed?.Invoke(this,EventArgs.Empty);
            Destroy(gameObject);
        }
    }
}
