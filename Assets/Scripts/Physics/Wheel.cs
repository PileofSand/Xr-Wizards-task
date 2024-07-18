using UnityEngine;

namespace Forklift.Physics
{
    [RequireComponent(typeof(Rigidbody))]
    public class Wheel : MonoBehaviour
    {
        private HingeJoint hingeJoint;
        private Rigidbody rb;

        private void Awake()
        {
            hingeJoint = GetComponent<HingeJoint>();
            rb = GetComponent<Rigidbody>();
        }

        public void ApplyMotor(float velocity)
        {
            if (velocity == 0)
            {
                return;
            }
            
            hingeJoint.useMotor = true;
            var motor = hingeJoint.motor;
            motor.targetVelocity = velocity;
            hingeJoint.motor = motor;
        }

        public void StopMotor()
        {
            var motor = hingeJoint.motor;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            motor.targetVelocity = 0f;
            hingeJoint.motor = motor;
            hingeJoint.useMotor = false;
        }
    }
}