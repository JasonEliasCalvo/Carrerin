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
    private float steerInput = 0f;
    private bool isAccelerating = false;
    private bool isBraking = false;
    private bool movementState = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.transform.parent = null;

        GameManager.instance.eventGameStart += ActiveMovement;
        GameManager.instance.eventGameEnd += DeactivateMovement;
    }

    public void ActiveMovement()
    {
        movementState = true;
    }
    public void DeactivateMovement()
    {
        movementState = false;
        rb.linearVelocity = Vector3.zero;
    }

    void Update()
    {
        // Animación de ruedas delanteras (solo visual)
        if (leftFrontWheel && rightFrontWheel)
        {
            leftFrontWheel.localRotation = Quaternion.Euler(
                leftFrontWheel.localRotation.eulerAngles.x,
                steerInput * maxWheelTurn,
                leftFrontWheel.localRotation.eulerAngles.z
            );

            rightFrontWheel.localRotation = Quaternion.Euler(
                rightFrontWheel.localRotation.eulerAngles.x,
                steerInput * maxWheelTurn,
                rightFrontWheel.localRotation.eulerAngles.z
            );
        }
    }

    void FixedUpdate()
    {
        CheckGround();
        LimitRotation();

        if (!movementState || !isGrounded)
            return;
        Vector3 moveDir = transform.forward;
        Vector3 velocityXZ = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z);
        float speed = velocityXZ.magnitude;

        float inputDirection = 0f;
        if (isAccelerating) inputDirection = 1f;
        else if (isBraking) inputDirection = -1f;

        float currentSpeedLimit = (inputDirection > 0) ? forwardSpeed : reverseSpeed;

        if (isMud && inputDirection > 0)
        {
            currentSpeedLimit *= acceleration;
        }

        // Solo aplicar fuerza si estamos acelerando o frenando
        if (inputDirection != 0 && speed < maxSpeed)
        {
            rb.AddForce(moveDir * currentSpeedLimit * inputDirection, ForceMode.Acceleration);

            float actualTurnSpeed = turnSpeed;
            if (inputDirection < 0)
                actualTurnSpeed *= reverseTurnMultiplier;

            float steerAmount = steerInput * actualTurnSpeed * Time.fixedDeltaTime;
            Quaternion turnOffset = Quaternion.Euler(0, steerAmount, 0);
            rb.MoveRotation(rb.rotation * turnOffset);
        }
        else
        {
            // Si no estamos acelerando ni frenando, frenar lentamente
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, 0.1f);
        }

        // Fricción general
        rb.linearVelocity *= drag;
    }

    public void OnSteer(InputValue value)
    {
        steerInput = value.Get<float>();
    }

    public void OnAccelerate(InputValue value)
    {
        isAccelerating = value.isPressed;
    }

    public void OnBrake(InputValue value)
    {
        isBraking = value.isPressed;
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
        if (currentItem == null)
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
