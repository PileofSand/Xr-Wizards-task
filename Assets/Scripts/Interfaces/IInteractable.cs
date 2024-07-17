namespace Forklift.Interfaces
{
    public interface IInteractable
    {
        public float Weight { get; }
        public void PickUp();
        public void Drop();
    }
}