using UnityEngine;

public class PlayerInteractionController : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ICollectible>(out var component))
        {
            component.Collect();
        }
    }
}
