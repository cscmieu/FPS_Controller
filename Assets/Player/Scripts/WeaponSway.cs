using UnityEngine;
using Quaternion = System.Numerics.Quaternion;

namespace Player.Scripts
{
    public class WeaponSway : MonoBehaviour
    {
        [Header("Settings", order = 0)] 
        [Header("Components", order = 1)] 
        
        [Header("Sway Variables", order = 2)]
        private float _swayDelay = 1f;
        private float _swaySnapBackSpeed = 1f;
        private Quaternion _targetRotation;
        
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        private void Update()
        {
            UpdateSway();
        }

        private void UpdateSway()
        {
            var mouseMovementX = Input.GetAxisRaw("Mouse X");
            var mouseMovementY = Input.GetAxisRaw("Mouse Y");
        }
    }
}
