using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform _OrientationTransform;
    [SerializeField] private float _movementSpeed;
    private Rigidbody _PlayerRigidbody;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private void Awake()
    {
        _PlayerRigidbody = GetComponent<Rigidbody>();
        _PlayerRigidbody.freezeRotation = true;

    }
    private void Update()
    {
        SetInputs();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

    }
    private void SetPlayerMovement()
    {
        _movementDirection = _OrientationTransform.forward * _verticalInput + _OrientationTransform.right * _horizontalInput;
        _PlayerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);

    }
}
