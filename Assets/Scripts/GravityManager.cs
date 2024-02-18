using UnityEngine;

public class GravityManager : MonoBehaviour
{
    [Header("Settings", order = 0)]
    public float gravityScale = 1;

    [Header("Components", order = 1)]
    private Transform _transform;

    [Header("Gravity Variables", order = 2)]
    private float _velocity;
    private float _groundedGravity   = 9.81f;
    private int   _floorLayerBitMask = 1 << 6;

    private void Awake()
    {
        _transform = transform;
    }

    public bool IsGrounded()
    {
        return Physics.Raycast(_transform.position, -_transform.up, 1f, _floorLayerBitMask);
    }
    
    private void Update()
    {
        if (IsGrounded())
        {
            _velocity = _groundedGravity;
        }
        else
        {
            _velocity -= _groundedGravity * gravityScale * Time.deltaTime;
        }
        ApplyGravity();
    }

    private void ApplyGravity()
    {
        _transform.position += new Vector3(0f, _velocity, 0f);
    }
}
