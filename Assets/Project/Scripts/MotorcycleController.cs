using NaughtyAttributes;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;

    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _steeringSpeed = 1f;
    [SerializeField] private float _rearWheelDistance = 2f;

    [SerializeField] private Transform _frontRaycastTransform;
    [SerializeField] private Transform _rearRaycastTransform;
    [SerializeField] private float _groundRaycastDistance = 1f;

    [SerializeField] private LayerMask _groundLayerMask;
    [SerializeField] private Transform _bikeMesh;

    private Vector3 _originalRotation;
    private Vector3 _originalPosition;

    private float _x;
    private float _y;
    private bool _isBreaking;

    private float _currentSpeed;

    private float _frontGroundDistance;
    private float _rearGroundDistance;

    private bool _frontRaycastHit;
    private bool _rearRaycastHit;
    private float _slopeAngle;

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
        _rigidbody.velocity = Vector3.Lerp(_rigidbody.velocity, _maxSpeed * _y * _rigidbody.transform.forward, Time.fixedDeltaTime * _acceleration);
        _rigidbody.angularVelocity = Vector3.zero; 
        transform.Rotate(0, _x * Time.fixedDeltaTime * _steeringSpeed, 0);
        RaycastSlope();
    }

    private void RaycastSlope()
    {
        Vector3 raycastDir = -Vector3.up * _groundRaycastDistance;

        if (Physics.Raycast(_frontRaycastTransform.position, -Vector3.up, out RaycastHit frontHit, _groundRaycastDistance, _groundLayerMask))
        {
            _frontRaycastHit = true;
            _frontGroundDistance = frontHit.distance;
            Debug.DrawRay(_frontRaycastTransform.position, raycastDir, Color.green);
        }
        else
        {
            Debug.DrawRay(_frontRaycastTransform.position, raycastDir, Color.red);
            _frontRaycastHit = false;
        }

        if (Physics.Raycast(_rearRaycastTransform.position, -Vector3.up, out RaycastHit rearHit, _groundRaycastDistance, _groundLayerMask))
        {
            _rearRaycastHit = true;
            _rearGroundDistance = rearHit.distance;
            Debug.DrawRay(_rearRaycastTransform.position, raycastDir, Color.green);
        }
        else
        {
            _frontRaycastHit = false;
            Debug.DrawRay(_rearRaycastTransform.position, raycastDir, Color.red);
        }

        if (!_frontRaycastHit || !_rearRaycastHit)
        {
            return;
        }

        _slopeAngle = Vector3.SignedAngle(frontHit.point - rearHit.point, transform.forward, transform.right);//Mathf.Rad2Deg * (Mathf.Atan2(frontHit.point.y - rearHit.point.y, frontHit.point.z - rearHit.point.z));

        Debug.DrawLine(
            frontHit.point,
            rearHit.point);

        Vector3 eulerAngles = _rigidbody.transform.eulerAngles;
        eulerAngles.x = -_slopeAngle;
        _bikeMesh.eulerAngles = eulerAngles;
    }

    private void GetInput()
    {
        _y = Input.GetAxis("Vertical");
        _x = Input.GetAxis("Horizontal");
        _isBreaking = Input.GetKey(KeyCode.Space);
    }

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
        GUILayout.TextArea($"Front Wheel V: {_rigidbody.velocity}\n" +
            $"Front Rotation: {_rigidbody.rotation}\n" +
            $"Slope Angle: {_slopeAngle}");
        GUILayout.EndArea();
    }
}
