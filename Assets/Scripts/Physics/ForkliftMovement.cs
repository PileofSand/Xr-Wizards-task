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
        private float currentVelocity = 0f;
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
            ApplyVelocityToWheels(currentVelocity);
            ApplySteering();
        }

        private void LateUpdate()
        {
            if (currentVelocity == 0) return;
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0f, staticSlowdown * Time.deltaTime);
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
            currentVelocity += acceleration * Time.deltaTime;
            currentVelocity = Mathf.Clamp(currentVelocity, maxBackwardSpeed, maxForwardSpeed);
        }

        private void Decelerate()
        {
            currentVelocity -= deceleration * Time.deltaTime;
            currentVelocity = Mathf.Clamp(currentVelocity, maxBackwardSpeed, maxForwardSpeed);
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
            currentVelocity = Mathf.MoveTowards(currentVelocity, 0f, brakePower * Time.deltaTime);

            if (Mathf.Approximately(currentVelocity, 0f))
            {
                StopTorqueOnWheels();
            }
        }

        private void ApplyVelocityToWheels(float velocity)
        {
            foreach (var wheel in wheels)
            {
                wheel.ApplyMotor(velocity);
            }
        }

        private void StopTorqueOnWheels()
        {
            foreach (var wheel in wheels)
            {
                wheel.StopMotor();
            }
        }
    }
}
