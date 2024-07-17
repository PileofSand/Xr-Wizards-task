using System;
using UnityEngine;
using Zenject;
using Forklift.Input;
using Forklift.Physics;

namespace Forklift.Controllers
{
    public class ForkliftController : MonoBehaviour
    {
        [SerializeField] private Transform _forkliftBody;
        
        private ForkliftInputHandler _inputHandler;
        private ForkliftMovement _movement;
        private Lift _lift;
        private CameraControl.CameraController _cameraController;

        [Inject]
        private void Construct(ForkliftInputHandler inputHandler, ForkliftMovement movement, Lift lift, CameraControl.CameraController cameraController)
        {
            _inputHandler = inputHandler;
            _movement = movement;
            _lift = lift;
            _cameraController = cameraController;
        }

        private void Awake()
        {
            _cameraController.FollowTarget(_forkliftBody);
        }

        private void Update()
        {
            HandleLift();
            HandleMovement();
            HandleTurning();
            HandleBraking();
        }

        private void HandleMovement()
        {
            if (_inputHandler.Move != 0)
            {
                _movement.HandleMovement(_inputHandler.Move);
            }
        }
        
        private void HandleBraking()
        {
            if (_inputHandler.Brake)
            {
                _movement.Brake();
            }
        }
        
        private void HandleTurning()
        {
            _movement.HandleTurning(_inputHandler.Turn);
        }

        private void HandleLift()
        {
            if (_inputHandler.RaiseLift)
            {
                _lift.Raise();
            }
            else if (_inputHandler.LowerLift)
            {
                _lift.Lower();
            }
        }
    }
}