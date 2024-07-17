using System;
using Forklift.Interfaces;
using UnityEngine;

namespace Forklift.Physics
{
    public class Lift : MonoBehaviour
    {
        [SerializeField] private Transform fork;
        [SerializeField] private float liftSpeed = 1f;
        [SerializeField] private float maxWeightCapacity = 100f;
        [SerializeField] private float maxHeight = 5f;
        [SerializeField] private float minHeight = 0f;
        private bool hasLoad = false;
        private float currentHeight = 0f;
        private const float heightOffset = 1.4f;
        private bool canLift = true;
        private IInteractable loadedItem;
        
        private void OnTriggerEnter(Collider other)
        {
            loadedItem = other.GetComponent<IInteractable>();
            if (loadedItem == null || hasLoad) return;
            
            hasLoad = true;
            if (loadedItem.Weight <= maxWeightCapacity)
            {
                loadedItem.PickUp();
                canLift = true;
            }
            else
            {
                canLift = false;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!hasLoad) return;
            
            loadedItem?.Drop();
            loadedItem = null;
            hasLoad = false;
            canLift = true;
        }

        public void Raise()
        {
            if (!canLift && currentHeight > minHeight + heightOffset)
            {
                return;
            }
            
            Vector3 newPosition = fork.localPosition;
            newPosition.y += liftSpeed * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);
            currentHeight = newPosition.y;
            fork.localPosition = newPosition;
        }

        public void Lower()
        {
            Vector3 newPosition = fork.localPosition;
            newPosition.y -= liftSpeed * Time.deltaTime;
            newPosition.y = Mathf.Clamp(newPosition.y, minHeight, maxHeight);
            currentHeight = newPosition.y;
            fork.localPosition = newPosition;
        }
    }
}