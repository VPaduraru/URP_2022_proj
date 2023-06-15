namespace SnowProject
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [Header("Movement values")]
        [SerializeField]
        private float _maxSpeed = 10f;
        [SerializeField]
        private float _minSpeed = 2f;
        private float _currentSpeed = 10f;
        private Vector3 _movementVector;
        [SerializeField]
        private GameObject _cameraPivot;
        private float _verticalRotation = 0f;
        private CharacterController _charController;
        [HideInInspector]
        public bool IsPlayerMoving { get; private set; } = false;
        private void Awake()
        {
            _charController = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        private void Update()
        {
            MouseRotation();
            MovementInput();

        }

        private void MovementInput()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            _movementVector = horizontal * transform.right + vertical * transform.forward;
            _movementVector = _currentSpeed * Time.deltaTime * _movementVector.normalized;
            if (Mathf.Abs(horizontal) > 0 || Mathf.Abs(vertical) > 0)
            {
                IsPlayerMoving = true;
            }
            else
            {
                IsPlayerMoving = false;
            }
        }

        private void Movement()
        {
            _charController.Move(_movementVector);
        }

        private void LateUpdate()
        {
            Movement();
        }

        private void MouseRotation()
        {
            _verticalRotation -= Input.GetAxis("Mouse Y");
            _verticalRotation = Mathf.Clamp(_verticalRotation, -20, 40);
            transform.Rotate(Vector3.up, Input.GetAxis("Mouse X"));
            _cameraPivot.transform.localEulerAngles = new Vector3(_verticalRotation, _cameraPivot.transform.localEulerAngles.y, _cameraPivot.transform.localEulerAngles.z);
        }

        public void SetSpeed01(float val)
        {
            _currentSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, val);
        }
    }
}

