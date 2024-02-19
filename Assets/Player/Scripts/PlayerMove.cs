using UnityEngine;

namespace Player.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        [SerializeField] private float movementSpeedMultiplier = 1f;

        [Header("Components", order = 1)] 
        private Transform           _transform;
        private CharacterController _characterController;

        [Header("Movement Variables", order = 2)]
        private const float movementSpeed = 12f;
        private const float airSlow = 0.7f;
        private Vector3 _pendingMovement;
        private float   _sideMovementX;
        private float   _forwardMovementZ;

        [Header("Gravity Variables", order = 3)]
        [SerializeField] private float gravityMultiplier = 5f;
        private const float gravity         = 9.81f;
        private const float groundedGravity = 2f;
        private       float _verticalVelocity;

        [Header("Jump Variables", order = 4)] 
        [SerializeField] private float jumpHeightMultiplier = 1f;
        private float _jumpHeight   = 2f;
        private float _jumpVelocity;
        

        private void Awake()
        {
            _transform           = transform;
            _characterController = GetComponent<CharacterController>();
            _jumpVelocity        = Mathf.Sqrt(_jumpHeight * jumpHeightMultiplier * 2f * gravity * gravityMultiplier);
        }

        private void Update()
        {
            _sideMovementX    = Input.GetAxisRaw("Horizontal");
            _forwardMovementZ = Input.GetAxisRaw("Vertical");
            ManageMovements();
            if (Input.GetKeyDown(KeyCode.Space))
                ManageJump();
            ApplyGravity();
        }
        
        private void LateUpdate()
        {
            _characterController.Move(_pendingMovement * Time.deltaTime);
        }

        private void ManageMovements()
        {
            _pendingMovement =  _transform.right * _sideMovementX + _transform.forward * _forwardMovementZ;
            _pendingMovement.Normalize();
            _pendingMovement *= movementSpeed * movementSpeedMultiplier;
            if (_characterController.isGrounded) return;
            _pendingMovement *= airSlow;
        }
        
        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _verticalVelocity < 0f)
            {
                _verticalVelocity = - groundedGravity * gravityMultiplier;
            }
            else
            {
                _verticalVelocity -= gravity * gravityMultiplier * Time.deltaTime;
            }
            _pendingMovement.y = _verticalVelocity;
        }

        private void ManageJump()
        {
            if (_characterController.isGrounded)
            {
                _verticalVelocity = _jumpVelocity;
            }
        }
    }
}
