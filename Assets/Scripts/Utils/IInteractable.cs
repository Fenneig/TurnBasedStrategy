using System;

namespace Utils
{
    public interface IInteractable
    {
        public void Interact(Action onInteractComplete);
    }
}