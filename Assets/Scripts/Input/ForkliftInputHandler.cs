using UnityEngine;
using UnityEngine.InputSystem;

namespace Forklift.Input
{
    public class ForkliftInputHandler : MonoBehaviour, ForkliftInput.IForkliftActions
    {
        public float Move { get; private set; }
        public float Turn { get; private set; }
        public bool RaiseLift { get; private set; }
        public bool LowerLift { get; private set; }
        public bool Brake { get; private set; }
        public Vector2 LookInput { get; private set; }

        private ForkliftInput _input;

        private void Awake()
        {
            _input = new ForkliftInput();
            _input.Forklift.SetCallbacks(this);
        }

        private void OnEnable()
        {
            _input.Forklift.Enable();
        }

        private void OnDisable()
        {
            _input.Forklift.Disable();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            Move = context.ReadValue<float>();
        }

        public void OnTurn(InputAction.CallbackContext context)
        {
            Turn = context.ReadValue<float>();
        }

        public void OnRaiseLift(InputAction.CallbackContext context)
        {
            RaiseLift = context.performed;
        }

        public void OnLowerLift(InputAction.CallbackContext context)
        {
            LowerLift = context.performed;
        }

        public void OnBrake(InputAction.CallbackContext context)
        {
            Brake = context.performed;
        }

        public void OnLookInput(InputAction.CallbackContext context)
        {
            LookInput = context.ReadValue<Vector2>();
        }
    }
}