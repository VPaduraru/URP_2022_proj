namespace SnowProject
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerController : MonoBehaviour
    {
        [Header("Movement values")]
        [SerializeField]
        private float _movementSpeed = 10f;
        private Vector3 _movementVector;
        [SerializeField]
        private GameObject _cameraPivot;
        private float _verticalRotation = 0f;
        private CharacterController _charController;
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

            _movementVector = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
            _movementVector = _movementSpeed * Time.deltaTime * _movementVector.normalized;


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
    }
}

