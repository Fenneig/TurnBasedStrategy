using System;

namespace Utils
{
    public interface IInteractable
    {
        void Interact(Action onInteractComplete);
    }
}