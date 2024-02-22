using NaughtyAttributes;
using UnityEngine;

public class MotorcycleController : MonoBehaviour
{
    [SerializeField] private Rigidbody _frontWheel;
    [SerializeField] private Rigidbody _rearWheel;

    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _acceleration = 2f;
    [SerializeField] private float _steeringSpeed = 1f;
    [SerializeField] private float _rearWheelDistance = 2f; 

    private Vector3 _originalRotation;
    private Vector3 _originalPosition;

    private float _x;
    private float _y;
    private bool _isBreaking;

    private float _currentSpeed;

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
        Vector3 velocity = Vector3.Lerp(_frontWheel.velocity, _maxSpeed * _y * _frontWheel.transform.forward, Time.fixedDeltaTime * _acceleration);

        Quaternion q = Quaternion.AngleAxis(_steeringSpeed * _x, transform.up);
        _frontWheel.MovePosition(q * (_frontWheel.transform.position - _rearWheel.position) + _rearWheel.position + velocity);
        _frontWheel.MoveRotation(_frontWheel.transform.rotation * q);

        _rearWheel.MovePosition(_frontWheel.position - _rearWheel.transform.forward * _rearWheelDistance);
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

        _frontWheel.velocity = Vector3.zero;
        _rearWheel.velocity = Vector3.zero;
    }

    private void OnGUI()
    {
        GUILayout.BeginArea(new Rect(new Vector2(10, 10), new Vector2(400, 400)));
        GUILayout.TextArea($"Front Wheel V: {_frontWheel.velocity}\n" +
            $"Front Rotation: {_frontWheel.rotation}");
        GUILayout.EndArea();
    }
}
