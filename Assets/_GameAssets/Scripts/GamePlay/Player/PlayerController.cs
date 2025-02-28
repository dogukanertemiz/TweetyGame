using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Referans")]
    [SerializeField] private Transform _OrientationTransform;

    [Header("Move Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;

    [Header("Jump Setting")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _moveJumpSpeed;
    [SerializeField] private float _jumpCoolDown;

    [Header("Ground Check Setting")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _playerHeight;

    [Header("Slider Setting")]
    [SerializeField] private KeyCode _sliderKey;
    [SerializeField] private float _sliderMoveSpeed;

    private Rigidbody _PlayerRigidbody;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private bool _canJum = true;
    private bool _isSlider;


    private void Awake()
    {
        _PlayerRigidbody = GetComponent<Rigidbody>();
        _PlayerRigidbody.freezeRotation = true;

    }
    private void Update()
    {
        SetInputs();
        LimitPlayerSpeed();
    }
    private void FixedUpdate()
    {
        SetPlayerMovement();
    }

    private void SetInputs()
    {
        _horizontalInput = Input.GetAxisRaw("Horizontal");
        _verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(_sliderKey))
        {
            _isSlider = true;
        }
        else if (Input.GetKeyDown(_movementKey))
        {
            _isSlider = false;

        }

        else if (Input.GetKey(_jumpKey) && _canJum && IsGrounded())
        {
            //KARAKTER ZIPLAMA
            _canJum = false;
            SetJumpMovement();
            Invoke(nameof(ResetJumping), _jumpCoolDown);
        }

    }
    private void SetPlayerMovement()
    {
        _movementDirection = _OrientationTransform.forward * _verticalInput + _OrientationTransform.right * _horizontalInput;
        if (_isSlider)
        {
            _PlayerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * _sliderMoveSpeed, ForceMode.Force);

        }
        else
        {
            _PlayerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed, ForceMode.Force);

        }

    }
    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_PlayerRigidbody.linearVelocity.x , 0f ,_PlayerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _PlayerRigidbody.linearVelocity = new Vector3(limitedVelocity.x , _PlayerRigidbody.linearVelocity.y,limitedVelocity.z);
        }

    }

    private void SetJumpMovement()
    {
        _PlayerRigidbody.linearVelocity = new Vector3(_PlayerRigidbody.linearVelocity.x, 0f, _PlayerRigidbody.linearVelocity.z);
        _PlayerRigidbody.AddForce(transform.up * _moveJumpSpeed, ForceMode.Impulse);

    }
    private void ResetJumping()
    {
        _canJum = true;

    }
    private bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _groundLayer);

    }
}
