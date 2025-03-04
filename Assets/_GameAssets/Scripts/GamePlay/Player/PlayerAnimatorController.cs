using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    [SerializeField] private Animator _playerAnimator;
    private PlayerController _playerController;
    private StateController _stateController;
    private void Awake()
    {
        _playerController = GetComponent<PlayerController>();
        _stateController = GetComponent<StateController>();

    }
    private void Start()
    {
        _playerController.OnPlayerJump += PlayerController_OnPlayerJump;
    }
    void Update()
    {
        SetPlayerAnimations();
    }
       private void PlayerController_OnPlayerJump()
    {
           _playerAnimator.SetBool("IsJumping", true);
           Invoke(nameof(ResetJumping),0.5f);
    }
     private void ResetJumping()
    {
           _playerAnimator.SetBool("IsJumping", false);
    }
    private void SetPlayerAnimations()
    {
        var currentState = _stateController.GetCurrentState();
        switch (currentState)
        {
            case PlayerState.ıdle:
            _playerAnimator.SetBool("IsSliding" , false);
            _playerAnimator.SetBool("IsMoving",false);
            break;
            case PlayerState.Move:
            _playerAnimator.SetBool("IsSliding" , false);
            _playerAnimator.SetBool("IsMoving",true);
            break;
            case PlayerState.SlideIdle:
            _playerAnimator.SetBool("IsSliding" , true);
            _playerAnimator.SetBool("IsMoving",false);
            break;
            case PlayerState.Slide:
            _playerAnimator.SetBool("IsSliding" , true);
            _playerAnimator.SetBool("IsMoving",true);
            break;
         

        }
    }
}
