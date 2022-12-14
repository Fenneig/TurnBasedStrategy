using System.Collections.Generic;
using UnityEngine;

namespace Utils
{
    public class RoomReveal : MonoBehaviour
    {
        [SerializeField] private List<GameObject> _fogOfWarList;
        [SerializeField] private List<GameObject> _enemyList;
        private bool _isRevealed;

        public void Reveal()
        {
            if (_isRevealed) return;
        
            foreach (var fogOfWar in _fogOfWarList)
                fogOfWar.SetActive(false);

            foreach (var enemy in _enemyList)
                enemy.SetActive(true);
        
            _isRevealed = true;
        }
    }
}