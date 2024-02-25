using UnityEngine;
using UnityEngine.Serialization;

namespace Player.Scripts
{
    public class PlayerLook : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        public float mouseSensitivityMultiplier = 1f;
        
        [Header("Components", order = 1)]
        [SerializeField] private Transform mainCameraHolder;

        [Header("Look Variables", order = 2)]
        private const float mouseSensitivityX = 300f;
        private const float mouseSensitivityY = 300f;
        private float _inputX;
        private float _inputY;
        private float _xRotation;
    
        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }


        private void Update()
        {
            _inputX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX * mouseSensitivityMultiplier * Time.deltaTime;
            _inputY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY * mouseSensitivityMultiplier * Time.deltaTime;
            Rotate();
        }

        private void Rotate()
        {
            _xRotation                          -= _inputY;
            _xRotation                          =  Mathf.Clamp(_xRotation, -90f, 90f);
            mainCameraHolder.localRotation =  Quaternion.Euler(new Vector3(_xRotation, 0f, 0f));
            transform.Rotate(Vector3.up, _inputX);
        }
    }
}
