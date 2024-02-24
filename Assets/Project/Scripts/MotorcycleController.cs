using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [Header("Properties")]
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _steeringSpeed = 1f;
    [SerializeField] private float _rearWheelDistance = 2f;

    [SerializeField] private float _groundRaycastDistance = 1f;
    [SerializeField] private float _isGroundedRaycastDistance = 0.5f;
    [SerializeField] private float _adjustRotationScale = 15f;

    [SerializeField] private LayerMask _groundLayerMask;
    
    [Header("Object References")]
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private Transform _frontRaycastTransform;
    [SerializeField] private Transform _rearRaycastTransform;

    [SerializeField] private Transform _bikeMesh;
    [SerializeField] private Transform _frontMeshGroup;

    private Vector3 _originalRotation;
    private Vector3 _originalPosition;
    private Vector3 _localVelocity;

    private float _x;
    private float _y;

    private bool _frontRaycastHit;
    private bool _rearRaycastHit;

    private float _slopeAngle;
    private bool _isGrounded;

    private void Awake()
    {
        _originalPosition = transform.position;
        _originalRotation = transform.eulerAngles;
    }

    private void Update()
    {
        GetInput();
    }

    private void FixedUpdate()
    {
        _localVelocity = transform.InverseTransformDirection(_rigidbody.velocity);
        _rigidbody.angularVelocity = Vector3.zero;

        int hasForwardVelocity = Utils.Math.Near(_rigidbody.velocity.magnitude, 0f, 0.1f) ? 0 : 1;
        int velocitySign = (int) Mathf.Sign(_localVelocity.z);

        _isGrounded = IsGrounded();

        if (_isGrounded)
        {
            _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, _maxSpeed * _y * _rigidbody.transform.forward, Time.fixedDeltaTime * _acceleration);

            _bikeMesh.Rotate(0, 0, 10 * -_x * hasForwardVelocity);
            _frontMeshGroup.localEulerAngles = new Vector3(-90f, _x * 25, 0);
        }

        transform.Rotate(0, _x * hasForwardVelocity * velocitySign * Time.fixedDeltaTime * _steeringSpeed, 0);

        RaycastSlope();
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(_frontRaycastTransform.position, -Vector3.up, _isGroundedRaycastDistance, _groundLayerMask)
            || Physics.Raycast(_rearRaycastTransform.position, -Vector3.up, _isGroundedRaycastDistance, _groundLayerMask);
    }

    private void RaycastSlope()
    {

        if (Physics.Raycast(_frontRaycastTransform.position, -Vector3.up, out RaycastHit frontHit, _groundRaycastDistance, _groundLayerMask))
        {
            _frontRaycastHit = true;
        }
        else
        {
            _frontRaycastHit = false;
        }

        if (Physics.Raycast(_rearRaycastTransform.position, -Vector3.up, out RaycastHit rearHit, _groundRaycastDistance, _groundLayerMask))
        {
            _rearRaycastHit = true;
        }
        else
        {
            _frontRaycastHit = false;
        }


        DrawRaycast(_frontRaycastTransform.position, _frontRaycastHit, _groundRaycastDistance);
        DrawRaycast(_rearRaycastTransform.position, _rearRaycastHit, _groundRaycastDistance);
        Debug.DrawLine(
        frontHit.point,
        rearHit.point);

        if (_frontRaycastHit && _rearRaycastHit)
        {
            _slopeAngle = Vector3.SignedAngle(frontHit.point - rearHit.point, transform.forward, transform.right);

            var rotation = _rigidbody.transform.rotation;
            rotation.eulerAngles = new(-_slopeAngle, rotation.eulerAngles.y, rotation.eulerAngles.z);
            _bikeMesh.rotation = Quaternion.Lerp(_bikeMesh.rotation, rotation, _adjustRotationScale * Time.fixedDeltaTime);
        }
    }

    private void GetInput()
    {
        _y = Input.GetAxis("Vertical");
        _x = Input.GetAxis("Horizontal");
    }

    #region Debug
    [Button]
    private void ResetAll()
    {
        transform.position = _originalPosition;
        transform.eulerAngles = _originalRotation;

        _rigidbody.velocity = Vector3.zero;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(new Vector2(10, 10), new Vector2(400, 400)));
        GUILayout.TextArea($"Velocity: {_rigidbody.velocity}\n" +
            $"Local Velocity: {_localVelocity}\n" +
            $"Rotation: {_rigidbody.rotation}\n" +
            $"Slope Angle: {_slopeAngle}\n" +
            $"Is Grouned: {_isGrounded}");
        GUILayout.EndArea();
    }

    private void DrawRaycast(Vector3 position, bool hit, float distance)
    {
        Vector3 raycastDir = -Vector3.up * distance;
        if (hit)
        {
            Debug.DrawRay(position, raycastDir, Color.green);
        }
        else
        {
            Debug.DrawRay(position, raycastDir, Color.red);
        }
    }
    #endregion
}
