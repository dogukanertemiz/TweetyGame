using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour , ICollectible
{
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private float _boostSpeed;
    [SerializeField] private float _resetBoostDuration;
    public void Collect()
    {
        _playerController.SetMovementSpeed(_boostSpeed, _resetBoostDuration);
        Destroy(gameObject);
    }
}
