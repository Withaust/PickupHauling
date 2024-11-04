using UnityEngine;

public class CollectTrigger : MonoBehaviour
{
    [SerializeField]
    public Player Player;

    [SerializeField]
    public LayerMask OnCollected;

    private int _counter = 0;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.TryGetComponent<Interactable>(out var interactComponent))
        {
            interactComponent.CanPickup = false;
            // Hacky, but I need to do this quick and submit it
            interactComponent.gameObject.layer = (int)Mathf.Log(OnCollected.value, 2);
            Player.Drop();
            _counter++;
            Player.UpdateCounter(_counter);
        }
    }
}
