using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;
using Forklift.Input;

namespace Forklift.CameraControl
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField] private float distance = 10.0f;
        [SerializeField] private float smoothSpeed = 0.125f;
        [SerializeField] private float rotationSpeed = 5.0f;

        private Transform target;
        private float currentX = 0.0f;
        private float currentY = 0.0f;
        private Vector3 direction;
        private Quaternion rotation;

        private ForkliftInputHandler _inputHandler;

        [Inject]
        public void Construct(ForkliftInputHandler inputHandler)
        {
            _inputHandler = inputHandler;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void FollowTarget(Transform followTarget)
        {
            target = followTarget;
        }

        private void Update()
        {
            HandleInput(_inputHandler.LookInput);
        }

        private void HandleInput(Vector2 lookInput)
        {
            currentX += lookInput.x * rotationSpeed * Time.deltaTime;
            currentY -= lookInput.y * rotationSpeed * Time.deltaTime;
            currentY = Mathf.Clamp(currentY, -35, 60);
        }

        private void LateUpdate()
        {
            if (target == null) return;

            direction = new Vector3(0, 0, -distance);
            rotation = Quaternion.Euler(currentY, currentX, 0);
            Vector3 desiredPosition = target.position + rotation * direction;

            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
            transform.LookAt(target.position);
        }
    }
}
