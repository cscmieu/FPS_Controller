using UnityEngine;

namespace Player.Scripts
{
    public class PlayerLook : MonoBehaviour
    {
        [Header("Settings", order = 0)]
        public float mouseSensitivityMultiplicator = 1f;
        
        [Header("Components", order = 1)]
        private Camera _mainCamera;

        [Header("Look Variables", order = 2)]
        private const float mouseSensitivityX = 300f;
        private const float mouseSensitivityY = 300f;
        private float _inputX;
        private float _inputY;
        private float _xRotation;
    
        private void Awake()
        {
            _mainCamera = Camera.main;
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible   = false;
        }


        private void Update()
        {
            _inputX = Input.GetAxisRaw("Mouse X") * mouseSensitivityX * mouseSensitivityMultiplicator * Time.deltaTime;
            _inputY = Input.GetAxisRaw("Mouse Y") * mouseSensitivityY * mouseSensitivityMultiplicator * Time.deltaTime;
            Rotate();
        }

        private void Rotate()
        {
            _xRotation                          -= _inputY;
            _xRotation                          =  Mathf.Clamp(_xRotation, -90f, 90f);
            _mainCamera.transform.localRotation =  Quaternion.Euler(new Vector3(_xRotation, 0f, 0f));
            transform.Rotate(Vector3.up, _inputX);
        }
    }
}
