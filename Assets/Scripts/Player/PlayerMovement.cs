using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 6f;
    public float sprintMultiplier = 1.6f;
    public float airControlMultiplier = 0.6f;

    [Header("Salto y Gravedad")]
    public float jumpHeight = 1.6f;
    public float gravity = -19.62f;
    public float groundedGravity = -2f;

    [Header("Dash")]
    public float dashForce = 20f;
    public float dashDuration = 0.15f;
    public float dashCooldown = 1f;

    // === DIOS MODE ===
    [Header("Modo Dios")]
    public float godModeSpeed = 10f;
    public KeyCode godModeToggleKey = KeyCode.C;

    private bool godMode = false;

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    private CharacterController controller;
    private AbilityManager abilityManager;

    private Vector3 velocity;
    private Vector3 inputDirection;

    private bool isGrounded;
    private bool canDoubleJump = false;

    private float coyote = 0.2f;
    private float coyoteCounter;

    private float jumpBuffer = 0.2f;
    private float jumpBufferCounter;

    public bool canMove = true;
    public RunScreenEffect runScreenEffect;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        abilityManager = AbilityManager.Instance;
    }

    void Update()
    {
        if (Input.GetKeyDown(godModeToggleKey))
            ToggleGodMode();

        if (!canMove)
            return;

        if (godMode)
        {
            GodModeMovement();
            return;
        }

        UpdateGroundStatus();

        if (isGrounded)
            coyoteCounter = coyote;
        else
            coyoteCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump"))
            jumpBufferCounter = jumpBuffer;
        else
            jumpBufferCounter -= Time.deltaTime;

        HandleDash();
        if (isDashing)
            return;

        ReadMovementInput();
        HandleJump();
        ApplyGravity();
        MovePlayer();
    }

    private void ToggleGodMode()
    {
        godMode = !godMode;
        velocity = Vector3.zero;
    }

    private void GodModeMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        float y = 0f;
        if (Input.GetKey(KeyCode.Space)) y = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) y = -1f;

        Vector3 move =
            transform.right * x +
            transform.forward * z +
            Vector3.up * y;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        float currentSpeed = godModeSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        controller.Move(move * currentSpeed * Time.deltaTime);
    }

    private void UpdateGroundStatus()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
            velocity.y = groundedGravity;
    }

    private void ReadMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        inputDirection = transform.right * x + transform.forward * z;

        if (inputDirection.sqrMagnitude > 1f)
            inputDirection.Normalize();

        if (!isGrounded)
            inputDirection *= airControlMultiplier;
    }

    private float GetCurrentSpeed()
    {
        float currentSpeed = speed;

        if (abilityManager.HasAbility(AbilityType.Run) && Input.GetKey(KeyCode.LeftShift))
            currentSpeed *= sprintMultiplier;

        return currentSpeed;
    }

    private void MovePlayer()
    {
        Vector3 move = inputDirection * GetCurrentSpeed();
        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (coyoteCounter > 0f)
        {
            canDoubleJump = true;

            if (jumpBufferCounter > 0f)
            {
                Jump();
                coyoteCounter = 0f;
                jumpBufferCounter = 0f;
            }
        }
        else if (abilityManager.HasAbility(AbilityType.DoubleJump))
        {
            if (canDoubleJump && Input.GetButtonDown("Jump"))
            {
                Jump();
                canDoubleJump = false;
            }
        }
    }

    private void Jump()
    {
        velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
    }

    private void ApplyGravity()
    {
        velocity.y += gravity * Time.deltaTime;
    }

    private void HandleDash()
    {
        if (!abilityManager.HasAbility(AbilityType.Dash))
            return;

        dashCooldownTimer -= Time.deltaTime;

        if (CanStartDash())
            StartDash();

        if (isDashing)
            ExecuteDash();
    }

    private bool CanStartDash()
    {
        return Input.GetKeyDown(KeyCode.R)
            && dashCooldownTimer <= 0f
            && !isDashing;
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
    }

    private void ExecuteDash()
    {
        dashTimer -= Time.deltaTime;

        Vector3 dashDirection = inputDirection != Vector3.zero ? inputDirection : transform.forward;
        controller.Move(dashDirection * dashForce * Time.deltaTime);

        if (dashTimer <= 0f)
            isDashing = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.collider.CompareTag("Lava"))
        {
            Destroy(gameObject);
        }
    }
}