using System.Collections.Generic;
using UnityEngine;

namespace Forklift.Physics
{
    public class ForkliftMovement : MonoBehaviour
    {
        [SerializeField] private float maxForwardSpeed = 10f;
        [SerializeField] private float maxBackwardSpeed = -5f;
        [SerializeField] private float acceleration = 5f;
        [SerializeField] private float deceleration = 5f;
        [SerializeField] private float brakePower = 10f;
        [SerializeField] private float staticSlowdown = 2f;
        
        [SerializeField] private Rigidbody _frontTires;
        [SerializeField] private ConfigurableJoint _frontAxisJoint;
        [SerializeField] private Wheel[] wheels;

        private Dictionary<TurnDirection, float> _targetTurnAngles;
        private float currentSpeed = 0f;
        private float currentTurnAngle = 0f;
        private float targetTurnAngle = 0f;

        private void Awake()
        {
            _targetTurnAngles = new Dictionary<TurnDirection, float>
            {
                {TurnDirection.None, 0f},
                {TurnDirection.Left, _frontAxisJoint.lowAngularXLimit.limit},
                {TurnDirection.Right, _frontAxisJoint.highAngularXLimit.limit}
            };
        }
        
        private void FixedUpdate()
        {
            ApplyTorqueToWheels(currentSpeed);
            ApplySteering();
        }

        private void LateUpdate()
        {
            if (currentSpeed == 0) return;
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, staticSlowdown * Time.deltaTime);
        }

        public void HandleMovement(float moveInput)
        {
            switch (moveInput)
            {
                case > 0:
                    Accelerate();
                    break;
                case < 0:
                    Decelerate();
                    break;
            }
        }

        public void HandleTurning(float turnInput)
        {
            if (turnInput != 0)
            {
                targetTurnAngle = _targetTurnAngles[turnInput > 0 ? TurnDirection.Right : TurnDirection.Left];
            }
            else
            {
                targetTurnAngle = _targetTurnAngles[TurnDirection.None];
            }
            currentTurnAngle = Mathf.Lerp(currentTurnAngle, targetTurnAngle, Time.deltaTime);
        }

        private void Accelerate()
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, maxBackwardSpeed, maxForwardSpeed);
        }

        private void Decelerate()
        {
            currentSpeed -= deceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, maxBackwardSpeed, maxForwardSpeed);
        }
        
        private void ApplySteering()
        {
            if (Mathf.Abs(currentTurnAngle) < 0.01f)
            {
                currentTurnAngle = 0f;
            }

            Vector3 targetFrontAxisRotation = Quaternion.Euler(0f, currentTurnAngle, 0f) * Vector3.forward;
            _frontTires.MoveRotation(Quaternion.LookRotation(targetFrontAxisRotation, _frontTires.transform.up));
        }

        public void Brake()
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakePower * Time.deltaTime);

            if (Mathf.Approximately(currentSpeed, 0f))
            {
                StopTorqueOnWheels();
            }
        }

        private void ApplyTorqueToWheels(float torque)
        {
            foreach (var wheel in wheels)
            {
                wheel.ApplyTorque(torque);
            }
        }

        private void StopTorqueOnWheels()
        {
            foreach (var wheel in wheels)
            {
                wheel.StopTorque();
            }
        }
    }
}
