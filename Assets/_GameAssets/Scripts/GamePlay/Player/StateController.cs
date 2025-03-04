using UnityEngine;

public class StateController : MonoBehaviour
{
    private PlayerState _currentPlayerState = PlayerState.ıdle;

    public void ChangeState(PlayerState newPlayerState)
    {
        if (_currentPlayerState == newPlayerState) { return; }
        _currentPlayerState = newPlayerState;
    }
    public PlayerState GetCurrentState()
    {
        return _currentPlayerState;

    }
}
