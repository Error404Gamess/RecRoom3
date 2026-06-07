using UnityEngine;
using UnityEngine.InputSystem;
using Mirror;

public class PlayerMovement : NetworkBehaviour
{
    public float speed = 5f;
    public float mouseSensitivity = 2f;

    Rigidbody rb;
    Camera cam;

    float xRotation = 0f;

    Vector2 moveInput;
    Vector2 lookInput;

    void Start()
    {
        if (!isLocalPlayer)
            return;

        rb = GetComponent<Rigidbody>();

        cam = GetComponentInChildren<Camera>();

        if (cam == null)
            cam = Camera.main;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        float mouseX = lookInput.x * mouseSensitivity;
        float mouseY = lookInput.y * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        if (cam != null)
        {
            cam.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer || rb == null || cam == null)
            return;

        Vector3 forward = cam.transform.forward;
        Vector3 right = cam.transform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 move = (right * moveInput.x + forward * moveInput.y).normalized;

        rb.MovePosition(
            rb.position + move * speed * Time.fixedDeltaTime
        );
    }

    public void OnMove(InputValue value)
    {
        if (!isLocalPlayer)
            return;

        moveInput = value.Get<Vector2>();
    }

    public void OnLook(InputValue value)
    {
        if (!isLocalPlayer)
            return;

        lookInput = value.Get<Vector2>();
    }
}