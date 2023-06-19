namespace SnowProject
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.Events;
    using UnityEngine.VFX;

    public class PlayerController : MonoBehaviour
    {
        [Header("VFX")]
        [SerializeField]
        private Transform _fromVfxTransform;
        [SerializeField]
        private VisualEffect[] _snowVFX;
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

        #region Gameplay Mechanics
        [Header("Gameplay Mechanics")]
        float _accumulationSnowMultiplier = 0.001f;
        float _currentPlaneHeight = 0;
        float _accumulatedSnow = 0f;

        #endregion

        #region Events
        public UnityEvent OnCollectSnow = new UnityEvent();
        #endregion
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
            StartCoroutine(VfxDebugThingy());
        }

        private IEnumerator VfxDebugThingy()
        {
            int index = 0;
            while (true)
            {
                _snowVFX[index].SetVector3("StartingPos", _fromVfxTransform.position);
                yield return new WaitForSeconds(1.0f);
                index++;
                if (index > 3)
                {
                    index = 0;
                }
            }
        }
        private void Update()
        {
            foreach (VisualEffect vfx in _snowVFX)
            {
                vfx.enabled = Input.GetButton("Fire1");
                vfx.SetVector3("EndingPos", transform.position + Vector3.up * 0.5f);
            }

            MouseRotation();
            MovementInput();
            float rightRotationAngle = transform.eulerAngles.y;
            if (Input.GetButton("Fire1"))
            {
                _accumulatedSnow += SnowController.Instance.PaintOnTextureCustomMaskFromPlayerRotation(transform.eulerAngles.y) * _accumulationSnowMultiplier;
                OnCollectSnow.Invoke();
            }
            //Debug.LogFormat("Forward: {0}, Right: {1}", forward, right);
            //Debug.LogFormat("rightRotationAngle: {0}", rightRotationAngle);

            _currentPlaneHeight = SnowController.Instance.GetCurrentPlaneHeight();
            SetSpeed01();

            if (IsPlayerMoving)
            {
                _accumulatedSnow += SnowController.Instance.PaintOnTexture() * _accumulationSnowMultiplier;
                OnCollectSnow.Invoke();
            }
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

        public float GetAccumulatedSnow()
        {
            return _accumulatedSnow;
        }

        private void SetSpeed01()
        {
            _currentSpeed = Mathf.Lerp(_minSpeed, _maxSpeed, -(Mathf.Cos(Mathf.PI * (1 - _currentPlaneHeight)) - 1) / 2);
        }
    }
}

