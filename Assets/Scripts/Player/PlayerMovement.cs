using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed = 6f;
    public float sprintMultiplier = 1.6f;
    public float airControlMultiplier = 0.6f;
    public float jumpHeight = 1.6f;
    public float gravity = -19.62f;
    public float groundedGravity = -2f;
    public float dashDistance = 8f;
    public float dashDuration = 1f;
    public float dashCooldown = 1f;
    public float godModeSpeed = 10f;
    public KeyCode godModeToggleKey = KeyCode.C;
    
    private bool godMode = false;
    private bool isDashing = false;
    private float dashTimer;
    private float dashCooldownTimer;
    private Vector3 dashVelocity;
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
    private Transform respawnPoint;
    public bool canMove = true;
    private bool isDying = false;
    private bool justLoadedScene = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        abilityManager = AbilityManager.Instance;
        respawnPoint = GameObject.FindWithTag("Respawn")?.transform;
        canMove = true;
        isDying = false;
        velocity = Vector3.zero;
        inputDirection = Vector3.zero;
    }

    void OnEnable() { SceneManager.sceneLoaded += OnSceneLoaded; }
    void OnDisable() { SceneManager.sceneLoaded -= OnSceneLoaded; }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        justLoadedScene = true;
        isDying = false;
        canMove = true;
        velocity = Vector3.zero;
        inputDirection = Vector3.zero;
        respawnPoint = GameObject.FindWithTag("Respawn")?.transform;
        StartCoroutine(DisableCCForOneFrame());
    }

    private IEnumerator DisableCCForOneFrame()
    {
        controller.enabled = false;
        yield return null;
        controller.enabled = true;
    }

    void Update()
    {
        if (Input.GetKeyDown(godModeToggleKey)) ToggleGodMode();

        if (godMode) { GodModeMovement(); return; }

        if (!canMove || !controller.enabled) return;

        UpdateGroundStatus();

        if (isGrounded) coyoteCounter = coyote;
        else coyoteCounter -= Time.deltaTime;

        if (Input.GetButtonDown("Jump")) jumpBufferCounter = jumpBuffer;
        else jumpBufferCounter -= Time.deltaTime;

        HandleDash();
        if (isDashing) return;

        ReadMovementInput();
        HandleJump();
        ApplyGravity();
        MovePlayer();

        justLoadedScene = false;
    }

    private void ToggleGodMode() { godMode = !godMode; velocity = Vector3.zero; }

    private void GodModeMovement()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        float y = 0f;
        if (Input.GetKey(KeyCode.Space)) y = 1f;
        if (Input.GetKey(KeyCode.LeftControl)) y = -1f;

        Vector3 move = transform.right * x + transform.forward * z + Vector3.up * y;
        if (move.sqrMagnitude > 1f) move.Normalize();
        controller.Move(move * godModeSpeed * Time.deltaTime);
    }

    private void UpdateGroundStatus()
    {
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0f) velocity.y = groundedGravity;
    }

    private void ReadMovementInput()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");
        inputDirection = transform.right * x + transform.forward * z;
        if (inputDirection.sqrMagnitude > 1f) inputDirection.Normalize();
        if (!isGrounded) inputDirection *= airControlMultiplier;
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
        if (!controller.enabled) return;
        Vector3 move = inputDirection * GetCurrentSpeed();
        move.y = velocity.y;
        controller.Move(move * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (coyoteCounter > 0f)
        {
            canDoubleJump = true;
            if (jumpBufferCounter > 0f) { Jump(); coyoteCounter = 0f; jumpBufferCounter = 0f; }
        }
        else if (abilityManager.HasAbility(AbilityType.DoubleJump))
        {
            if (canDoubleJump && Input.GetButtonDown("Jump")) { Jump(); canDoubleJump = false; }
        }
    }

    private void Jump() { velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity); }

    private void ApplyGravity() { velocity.y += gravity * Time.deltaTime; }

    private void HandleDash()
    {
        if (!abilityManager.HasAbility(AbilityType.Dash)) return;
        dashCooldownTimer -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.R) && dashCooldownTimer <= 0f && !isDashing) StartDash();
        if (isDashing) ExecuteDash();
    }

    private void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        Vector3 dashDirection = inputDirection != Vector3.zero ? inputDirection.normalized : transform.forward;
        dashVelocity = dashDirection * (dashDistance / dashDuration);
    }

    private void ExecuteDash()
    {
        if (!controller.enabled) return;
        dashTimer -= Time.deltaTime;
        controller.Move(dashVelocity * Time.deltaTime);
        if (dashTimer <= 0f) isDashing = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Lava") && respawnPoint != null)
        {
            controller.enabled = false;
            transform.position = respawnPoint.position;
            velocity = Vector3.zero;
            controller.enabled = true;
        }
    }

    public void SetRespawnPoint(Transform newPoint)
    {
        respawnPoint = newPoint;
    }

    public void ResetDeathState()
    {
        StopAllCoroutines();
        isDying = false;
        canMove = true;
    }
}