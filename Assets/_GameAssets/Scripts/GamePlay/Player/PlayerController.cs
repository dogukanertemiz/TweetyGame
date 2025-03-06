using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public event Action OnPlayerJump;
    [Header("Referans")]
    [SerializeField] private Transform _OrientationTransform;

    [Header("Move Settings")]
    [SerializeField] private KeyCode _movementKey;
    [SerializeField] private float _movementSpeed;

    [Header("Jump Setting")]
    [SerializeField] private KeyCode _jumpKey;
    [SerializeField] private float _moveJumpSpeed;
    [SerializeField] private float _jumpCoolDown;
    [SerializeField] private float _airMultiplier;

    [Header("Ground Check Setting")]
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private float _playerHeight;

    [Header("Slider Setting")]
    [SerializeField] private KeyCode _sliderKey;
    [SerializeField] private float _sliderMoveSpeed;

    private StateController _stateController;
    private float _startingMovement, _startingJumping;
    private Rigidbody _PlayerRigidbody;
    private float _horizontalInput, _verticalInput;
    private Vector3 _movementDirection;
    private bool _canJum = true;
    private bool _isSlider;


    private void Awake()

    {
        _PlayerRigidbody = GetComponent<Rigidbody>();
        _PlayerRigidbody.freezeRotation = true;
        _stateController = GetComponent<StateController>();
        _startingMovement = _movementSpeed;
        _startingJumping = _moveJumpSpeed;

    }


    private void Update()
    {
        SetInputs();
        LimitPlayerSpeed();
        SetStates();
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
    private void SetStates()
    {
        var _movementDirection = GetMovementDirection();
        var isGrounded = IsGrounded();
        var currentState = _stateController.GetCurrentState();

        var newState = currentState switch
        {
            _ when _movementDirection == Vector3.zero && isGrounded && !_isSlider => PlayerState.ıdle,
            _ when _movementDirection != Vector3.zero && isGrounded && !_isSlider => PlayerState.Move,
            _ when _movementDirection != Vector3.zero && isGrounded && _isSlider => PlayerState.Slide,
            _ when _movementDirection == Vector3.zero && isGrounded && _isSlider => PlayerState.SlideIdle,
            _ when !_canJum && !isGrounded => PlayerState.Jump,
            _ => currentState
        };
        if (newState != currentState)
        {
            _stateController.ChangeState(newState);
        }


    }
    private void SetPlayerMovement()
    {
        _movementDirection = _OrientationTransform.forward * _verticalInput + _OrientationTransform.right * _horizontalInput;
        float forceMultiplier = _stateController.GetCurrentState() switch
        {
            PlayerState.Move => 1f,
            PlayerState.Slide => _sliderMoveSpeed,
            PlayerState.Jump => _airMultiplier,
            _ => 1f

        };
        _PlayerRigidbody.AddForce(_movementDirection.normalized * _movementSpeed * forceMultiplier, ForceMode.Force);


    }
    private void LimitPlayerSpeed()
    {
        Vector3 flatVelocity = new Vector3(_PlayerRigidbody.linearVelocity.x, 0f, _PlayerRigidbody.linearVelocity.z);
        if (flatVelocity.magnitude > _movementSpeed)
        {
            Vector3 limitedVelocity = flatVelocity.normalized * _movementSpeed;
            _PlayerRigidbody.linearVelocity = new Vector3(limitedVelocity.x, _PlayerRigidbody.linearVelocity.y, limitedVelocity.z);
        }

    }

    private void SetJumpMovement()
    {
        OnPlayerJump?.Invoke();
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
    private Vector3 GetMovementDirection()
    {
        return _movementDirection.normalized;

    }

    public void SetMovementSpeed(float speed , float duration)
    {
        _movementSpeed += speed;
        Invoke(nameof(ResetMovementSpeed),duration);

    }
    public void ResetMovementSpeed()
    {
        _movementSpeed = _startingMovement;

    }
    public void SetJumpForce(float force , float duration)
    {
        _moveJumpSpeed += force;
        Invoke(nameof(ResetJumpForce),duration);

    }
    public void ResetJumpForce()
    {
        _moveJumpSpeed = _startingJumping;

    }
}
