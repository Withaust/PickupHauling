using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public float Speed = 7.5f;
    public float Gravity = 20.0f;
    public float Sensitivity = 2.0f;
    public GameObject Head;
    public GameObject Hands;

    public TMP_Text InteractText;
    public TMP_Text CounterText;
    public Image VisualCursor;

    public LayerMask InteractMask;

    CharacterController _controller;
    Vector3 _motion = Vector3.zero;
    Vector2 _rotation = Vector2.zero;

    private RaycastHit _raycastHit;
    private Interactable _heldInteractable = null;
    private bool _canInteract = false;

    const string MouseX = "Mouse X";
    const string MouseY = "Mouse Y";
    const string Vertical = "Vertical";
    const string Horizontal = "Horizontal";
    const string Fire1 = "Fire1";

    void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        Vector3 forward = Head.transform.TransformDirection(Vector3.forward);
        Vector3 right = Head.transform.TransformDirection(Vector3.right);

        float curSpeedX = Speed * Input.GetAxis(Vertical);
        float curSpeedY = Speed * Input.GetAxis(Horizontal);

        float movementDirectionY = _motion.y;
        _motion = (forward * curSpeedX) + (right * curSpeedY);

        if (_controller.isGrounded)
        {
            _motion.y = movementDirectionY;
        }
        else
        {
            _motion.y -= Gravity * Time.deltaTime;
        }

        _controller.Move(_motion * Time.deltaTime);

        _rotation.x += Input.GetAxis(MouseX) * Sensitivity;
        _rotation.y += Input.GetAxis(MouseY) * Sensitivity;
        _rotation.y = Mathf.Clamp(_rotation.y, -89.0f, 89.0f);
        var xQuat = Quaternion.AngleAxis(_rotation.x, Vector3.up);
        var yQuat = Quaternion.AngleAxis(_rotation.y, Vector3.left);

        Head.transform.localRotation = xQuat * yQuat;

        if(_heldInteractable)
        {
            InteractText.enabled = true;
            InteractText.text = "Press E / LMB to drop object";

            if (Input.GetButtonDown(Fire1))
            {
                Drop();
            }
        }
        else
        {
            if(_canInteract)
            {
                if (_raycastHit.collider.transform.TryGetComponent<Interactable>(out var Interactable))
                {
                    if(Interactable.CanPickup)
                    {
                        VisualCursor.color = Color.green;
                        InteractText.enabled = true;
                        InteractText.text = "Press E / LMB to pickup object";

                        if (Input.GetButtonDown(Fire1))
                        {
                            Pickup(Interactable);
                        }
                    }
                }
            }
            else
            {
                VisualCursor.color = Color.white;
                InteractText.enabled = false;
            }
        }
    }

    public void Drop()
    {
        if(_heldInteractable)
        {
            _heldInteractable.Drop();
            _heldInteractable = null;
        }
    }

    public void Pickup(Interactable target)
    {
        target.Pickup(this);
        _heldInteractable = target;
    }

    public void UpdateCounter(int newCount)
    {
        CounterText.text = "Collected: " + newCount + " items!";
    }

    private void FixedUpdate()
    {
        Ray ray = new Ray(Head.transform.position, Head.transform.forward);
        _canInteract = Physics.Raycast(ray, out _raycastHit, 5.0f, InteractMask);
    }
}