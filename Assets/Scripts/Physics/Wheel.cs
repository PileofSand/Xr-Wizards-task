using UnityEngine;

namespace Forklift.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Wheel : MonoBehaviour
    {
        private Rigidbody rb;

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
        }

        public void ApplyTorque(float force)
        {
            rb.AddTorque(transform.right * force, ForceMode.Force);
        }

        public void StopTorque()
        {
            rb.angularVelocity = Vector3.zero;
            rb.velocity = Vector3.zero;
        }
    }
}