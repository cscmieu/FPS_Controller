using UnityEngine;

namespace Player.Scripts
{
    public class PlayerMove : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        public float movementSpeedMultiplicator = 1f;

        [Header("Components", order = 1)] 
        private Transform _transform;
        private Rigidbody _rigidBody;
        
        [Header("Movement Variables", order = 2)]
        private const float movementSpeed = 12f;
        private float _sideMovementX;
        private float _forwardMovementZ;


        private void Awake()
        {
            _transform           = transform;
            _rigidBody = GetComponent<Rigidbody>();
        }

        private void Update()
        {
            _sideMovementX = Input.GetAxisRaw("Horizontal");
            _forwardMovementZ = Input.GetAxisRaw("Vertical");
            Move();
        }

        private void Move()
        {
            var pendingMovement = _transform.right * _sideMovementX + _transform.forward * _forwardMovementZ;
            _transform.position += pendingMovement * (movementSpeed * movementSpeedMultiplicator * Time.deltaTime);
        }
    }
}
