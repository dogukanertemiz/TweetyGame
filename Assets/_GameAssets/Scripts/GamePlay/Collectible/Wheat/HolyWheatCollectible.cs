using UnityEngine;

public class HolyWheatCollectible : MonoBehaviour, ICollectible
{
    [SerializeField] private WheatDesingSO _wheatDesingSO;
    [SerializeField] private PlayerController _playerController;
    public void Collect()
    {
        _playerController.SetJumpForce(_wheatDesingSO._ıncreaseSpeedBoost, _wheatDesingSO._resetBoostDuration);
        Destroy(gameObject);
    }
}
