using UnityEngine;

public class KartController : MonoBehaviour
{
    [Header("Movimiento")]
    public float acceleration = 150f;
    public float maxSpeed = 100;
    public float turnSpeed = 100f;
    public float drag = 0.95f;
    public GameObject cam;

    private Rigidbody rb;
    private PlayerControls controls;
    private Vector2 moveInput;
    private bool movementState = true;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        controls.Player.Move.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        controls.Player.Move.canceled += ctx => moveInput = Vector2.zero;
    }

    private void OnEnable() => controls.Player.Enable();
    private void OnDisable() => controls.Player.Disable();

    void FixedUpdate()
    {
        Vector3 moveDir = GetMoveDirection();

        if (moveDir.sqrMagnitude > 0.01f)
        {
            // Aceleración
            if (rb.linearVelocity.magnitude < maxSpeed)
            {
                rb.AddForce(moveDir * acceleration * Time.fixedDeltaTime, ForceMode.Acceleration);
            }

            // Rotación hacia la dirección del movimiento
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, targetRotation, turnSpeed * Time.fixedDeltaTime));
        }

        // Fricción manual
        rb.linearVelocity *= drag;
    }

    public Vector3 GetMoveDirection()
    {
        if (!movementState) return Vector3.zero;

        Vector3 cameraForward = cam.transform.forward;
        cameraForward.y = 0f;
        cameraForward.Normalize();

        Vector3 cameraRight = cam.transform.right;
        cameraRight.y = 0f;
        cameraRight.Normalize();

        Vector3 moveDir = (cameraForward * moveInput.y + cameraRight * moveInput.x);
        if (moveDir.magnitude > 1f)
            moveDir.Normalize();

        return moveDir;
    }
}
