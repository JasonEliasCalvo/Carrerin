using UnityEngine;
using UnityEngine.InputSystem;

public class KartController : MonoBehaviour
{
    [Header("Velocidades")]
    public float forwardSpeed = 50;
    public float reverseSpeed = 20f;
    public float maxSpeed = 100;
    public float acceleration = 3f;

    [Header("Giro")]
    public float turnSpeed = 10f;
    public float reverseTurnMultiplier = 0.5f;

    [Header("Física")]
    public float drag = 0.95f;
    public float gravityForce = -10f;

    [Header("Detección de suelo")]
    public Transform groundCheck;
    public Vector3 boxSize = new Vector3(0.5f, 0.1f, 0.5f);
    public LayerMask whatIsGround;
    public LayerMask whatIsMud;
    private bool isGrounded;
    private bool isMud;

    [Header("Ruedas delanteras (opcional)")]
    public Transform leftFrontWheel;
    public Transform rightFrontWheel;
    public float maxWheelTurn = 30f;

    [Header("Cámara")]
    public GameObject cam;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool movementState = true;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.transform.parent = null;
    }

    void Update()
    {
        // Animación de ruedas delanteras (solo visual)
        if (leftFrontWheel && rightFrontWheel)
        {
            leftFrontWheel.localRotation = Quaternion.Euler(
                leftFrontWheel.localRotation.eulerAngles.x,
                moveInput.x * maxWheelTurn,
                leftFrontWheel.localRotation.eulerAngles.z
            );

            rightFrontWheel.localRotation = Quaternion.Euler(
                rightFrontWheel.localRotation.eulerAngles.x,
                moveInput.x * maxWheelTurn,
                rightFrontWheel.localRotation.eulerAngles.z
            );
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        LimitRotation();
        Vector3 moveDir = GetMoveDirection();
        Vector3 velocityXZ = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);

        float currentSpeedLimit = (moveInput.y > 0) ? forwardSpeed : reverseSpeed;

        if (isMud && moveInput.y > 0)
        {
            currentSpeedLimit *= acceleration;
        }

        if (moveDir.sqrMagnitude > 0.01f && velocityXZ.magnitude < maxSpeed)
        {
            rb.AddForce(moveDir * currentSpeedLimit, ForceMode.Acceleration);

            // Ajustar velocidad
            float actualTurnSpeed = turnSpeed;
            if (moveInput.y < 0)
            {
                actualTurnSpeed *= reverseTurnMultiplier;
            }

            // Rotación suave
            if (moveInput.y > 0)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                rb.MoveRotation(Quaternion.Slerp(
                    rb.rotation,
                    targetRotation,
                    actualTurnSpeed * Time.fixedDeltaTime
                ));
            }
        }

        // Fricción manual
        rb.linearVelocity *= drag;
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
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

    public void LimitRotation()
    {
        Vector3 currentRotation = rb.rotation.eulerAngles;
        float tiltX = Mathf.DeltaAngle(0, currentRotation.x);
        float tiltZ = Mathf.DeltaAngle(0, currentRotation.z);

        float maxTilt = 30f;

        if (Mathf.Abs(tiltX) > maxTilt || Mathf.Abs(tiltZ) > maxTilt)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
        }
        else
        {
            rb.constraints = RigidbodyConstraints.None | RigidbodyConstraints.FreezeRotationY;
        }
    }

    public void CheckGround()
    {
        isGrounded = Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, whatIsGround);

        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * gravityForce, ForceMode.Acceleration);
            return;
        }

        isMud = Physics.CheckBox(groundCheck.position, boxSize, Quaternion.identity, whatIsMud);
    }

    private InteractableOptions currentItem;

    public void PickupItem(InteractableOptions item)
    {
        if (currentItem == null) // solo si no tienes uno ya
        {
            currentItem = item;
            item.gameObject.SetActive(false);
        }
    }

    public void UseItem()
    {
        if (currentItem != null)
        {
            currentItem.Use(this);
            currentItem = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(groundCheck.position, boxSize * 2);
        }
    }
}
