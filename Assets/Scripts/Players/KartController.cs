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

    [Header("Ruedas")]
    public Transform leftFrontWheel;
    public Transform rightFrontWheel;
    public float maxWheelTurn = 30f;

    [Header("Cámara")]
    public GameObject cam;

    // Estado interno
    private Rigidbody rb;
    private float steerInput;
    private bool isAccelerating;
    private bool isBraking;
    private bool movementState;
    private bool isGrounded;
    private bool isMud;

    private const float tiltLimit = 25f;
    private const float brakeThreshold = 0.2f;
    private const float accelThreshold = 0.2f;

    private InteractableOptions currentItem;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.centerOfMass = new Vector3(0, -0.5f, 0);
        rb.transform.parent = null;

        GameManager.instance.eventGameStart += ActiveMovement;
        GameManager.instance.eventGameEnd += DeactivateMovement;
    }

    private void Update()
    {
        UpdateWheelVisuals();
    }

    private void FixedUpdate()
    {
        CheckGround();
        LimitRotation();

        if (!movementState || !isGrounded)
            return;

        ApplyMovement();
    }

    public void OnSteer(InputValue value)
    {
        steerInput = value.Get<float>();
    }

    public void OnAccelerate(InputValue value)
    {
        isAccelerating = value.Get<float>() > accelThreshold;
    }

    public void OnBrake(InputValue value)
    {
        isBraking = value.Get<float>() > brakeThreshold;
    }

    public void ActiveMovement() => movementState = true;

    public void DeactivateMovement()
    {
        movementState = false;
        rb.linearVelocity = Vector3.zero;
    }

    private void ApplyMovement()
    {
        Vector3 moveDir = transform.forward;
        float speed = new Vector3(rb.linearVelocity.x, 0, rb.linearVelocity.z).magnitude;

        float inputDirection = 0;

        if (isAccelerating && isBraking) inputDirection = 0;
        else if (isAccelerating) inputDirection = 1f;
        else if (isBraking) inputDirection = -1f;

        float currentSpeedLimit = inputDirection > 0 ? forwardSpeed : reverseSpeed;

        if (isMud && inputDirection > 0)
            currentSpeedLimit *= acceleration;

        if (inputDirection != 0 && speed < maxSpeed)
        {
            rb.AddForce(moveDir * currentSpeedLimit * inputDirection, ForceMode.Acceleration);

            float actualTurnSpeed = inputDirection < 0 ? turnSpeed * reverseTurnMultiplier : turnSpeed;
            float steerAmount = steerInput * actualTurnSpeed * Time.fixedDeltaTime;

            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, steerAmount, 0));
        }
        else
        {
            rb.linearVelocity = Vector3.Lerp(rb.linearVelocity, Vector3.zero, 0.05f);
        }

        rb.linearVelocity *= drag;
    }

    private void UpdateWheelVisuals()
    {
        if (!leftFrontWheel || !rightFrontWheel) return;

        float wheelRotation = steerInput * maxWheelTurn;

        leftFrontWheel.localRotation = Quaternion.Euler(
            leftFrontWheel.localRotation.eulerAngles.x, wheelRotation, leftFrontWheel.localRotation.eulerAngles.z
        );

        rightFrontWheel.localRotation = Quaternion.Euler(
            rightFrontWheel.localRotation.eulerAngles.x, wheelRotation, rightFrontWheel.localRotation.eulerAngles.z
        );
    }

   

    public void LimitRotation()
    {
        Vector3 currentRotation = rb.rotation.eulerAngles;
        float tiltX = Mathf.DeltaAngle(0, currentRotation.x);
        float tiltZ = Mathf.DeltaAngle(0, currentRotation.z);

        if (Mathf.Abs(tiltX) > tiltLimit || Mathf.Abs(tiltZ) > tiltLimit)
        {
            rb.constraints = RigidbodyConstraints.FreezeRotationX |
                             RigidbodyConstraints.FreezeRotationZ |
                             RigidbodyConstraints.FreezeRotationY;
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

    public void PickupItem(InteractableOptions item)
    {
        if (currentItem == null)
        {
            currentItem = item;
        }
        else
        {
            currentItem = null;
            currentItem = item;
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
