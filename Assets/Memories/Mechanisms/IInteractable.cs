namespace Memories.Mechanisms
{
    public interface IInteractable
    {
        bool CanInteract(Characters.Player player);
        void Interact(Characters.Player player);
        void DiscardAfterUse(Characters.Player player) { }
    }
}
