using UnityEngine;
using Forklift.Interfaces;

namespace Forklift.Models
{
    public class PhysicsObject : MonoBehaviour, IInteractable
    {
        [SerializeField] private float weight = 50f;

        private bool isPickedUp = false;
        public float Weight => weight;
        
        public void PickUp()
        {
            isPickedUp = true;
        }

        public void Drop()
        {
            isPickedUp = false;
        }
    }
}