using UnityEngine;

public class GoldWheatCollectible : MonoBehaviour , ICollectible
{
    [SerializeField] private WheatDesingSO _wheatDesingSO;
    [SerializeField] private PlayerController _playerController;
    public void Collect()
    {
        _playerController.SetMovementSpeed(_wheatDesingSO._ıncreaseSpeedBoost, _wheatDesingSO._resetBoostDuration);
        Destroy(gameObject);
    }
}
