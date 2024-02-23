using UnityEngine;

namespace Player.Scripts
{
    public class WeaponSwayBobAndTilt : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        
        [Header("Components", order = 1)]

        private Transform _transform;
        private Quaternion _originRotation;
        private Vector3    _originPosition;

        [Header("Sway, Bob & Tilt Variables", order = 2)]
        
        private float _bobSpeed           = 5f;

        private float _forwardBobIntensity = .1f;
        private float _sideBobIntensity = .1f;
        private float _verticalBobIntensity = .05f;
        private float _bobFrequency        = 15f;
        private float _swayIntensity       = 10f;
        private float _tiltIntensity       = 5f;
        private float _swayTiltResetSpeed  = 5f;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            _originRotation = _transform.localRotation;
            _originPosition = _transform.localPosition;
        }

        private void Update()
        {
            UpdateSwayBobAndTilt();
        }

        private void UpdateSwayBobAndTilt()
        {
            //Input Getters
            var lookRight = Input.GetAxisRaw("Mouse X");
            var lookUp = Input.GetAxisRaw("Mouse Y");
            var moveRight    = Input.GetAxisRaw("Horizontal");
            var moveForward = Input.GetAxisRaw("Vertical");

            //Bob
            var moveAmount = Mathf.Clamp(moveForward + moveRight, 0.3f, 1f);
            var verticalBob = Mathf.Sin(Time.time * _bobFrequency) * _verticalBobIntensity * moveAmount;
            var totalBob    = new Vector3(-moveRight * _sideBobIntensity, verticalBob, -moveForward * _forwardBobIntensity);

            //Sway
            var horizontalSway = Quaternion.AngleAxis(-lookRight * _swayIntensity, Vector3.up);
            var verticalSway   = Quaternion.AngleAxis(lookUp * _swayIntensity, Vector3.right);
            var totalSway      = horizontalSway * verticalSway;
            
            //Tilt
            var totalTilt = Quaternion.AngleAxis(-lookRight * _tiltIntensity, Vector3.forward);

            //Additions
            var targetRotation = _originRotation * totalTilt * totalSway;
            var targetPosition = _originPosition + totalBob;
            
            _transform.localRotation = Quaternion.Lerp(_transform.localRotation, targetRotation, Time.deltaTime * _swayTiltResetSpeed);
            _transform.localPosition = Vector3.Lerp(_transform.localPosition, targetPosition, Time.deltaTime * _bobSpeed);
        }
    }
}
