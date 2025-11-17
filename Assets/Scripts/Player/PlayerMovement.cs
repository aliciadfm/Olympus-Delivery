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

    private bool isDashing = false;
    private float dashTimer = 0f;
    private float dashCooldownTimer = 0f;

    private CharacterController controller;
    private AbilityManager abilityManager;

    private Vector3 velocity;
    private Vector3 inputDirection;

    private bool isGrounded;
    private bool canDoubleJump = false;

    public bool canMove = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        abilityManager = FindAnyObjectByType<AbilityManager>();
    }

    void Update()
    {
        if (!canMove) return;

        EnsureAbilityManager();
        UpdateGroundStatus();
        HandleDash();

        if (isDashing) return;

        ReadMovementInput();
        MovePlayer();
        HandleJump();
        ApplyGravity();
    }

    private void EnsureAbilityManager()
    {
        if (abilityManager == null)
            abilityManager = FindAnyObjectByType<AbilityManager>();
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

        inputDirection = (transform.right * x + transform.forward * z);

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
        float currentSpeed = GetCurrentSpeed();
        controller.Move(inputDirection * currentSpeed * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isGrounded)
        {
            canDoubleJump = true;

            if (Input.GetButtonDown("Jump"))
                Jump();
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
        controller.Move(velocity * Time.deltaTime);
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

        controller.Move(transform.forward * dashForce * Time.deltaTime);

        if (dashTimer <= 0f)
            isDashing = false;
    }
}