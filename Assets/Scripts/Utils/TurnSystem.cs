using System;
using UnityEngine;

namespace Utils
{
    public class TurnSystem : MonoBehaviour
    {
        public static TurnSystem Instance { get; private set; }

        private int _turnNumber = 1;
        private bool _isPlayerTurn = true;

        public bool IsPlayerTurn => _isPlayerTurn;
    
        public int TurnNumber => _turnNumber;

        public event EventHandler OnTurnChanged;

        private void Awake()
        {
            Instance ??= this;
        }

        public void NextTurn()
        {
            _turnNumber++;
            _isPlayerTurn = !_isPlayerTurn;
        
            OnTurnChanged?.Invoke(this,EventArgs.Empty);
        }
    
    
    }
}