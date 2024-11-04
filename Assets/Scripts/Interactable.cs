using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool CanPickup = true;

    private Rigidbody _rigidbody = null;
    private GameObject _target = null;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (_target == null)
        {
            _rigidbody.useGravity = true;
        }
        else
        {
            // I know that this should be done in FixedUpdate for phys objects, but it will look jittery
            _rigidbody.transform.SetPositionAndRotation(
                Vector3.Lerp(_rigidbody.transform.position, _target.transform.position, 0.25f),
                Quaternion.Lerp(_rigidbody.transform.rotation, Quaternion.identity, 0.4f));
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.angularVelocity = Vector3.zero;
        }
    }

    public void Pickup(Player player)
    {
        _rigidbody.useGravity = false;
        _target = player.Hands.gameObject;
    }

    public void Drop()
    {
        _rigidbody.useGravity = true;
        _target = null;
    }
}