using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movimiento")]
    public float moveSpeed = 6f;

    [Header("Salto y Gravedad")]
    public float jumpHeight = 1.6f;
    public float gravity = -19.62f; // ~2x gravedad real para feeling FPS
    public float groundedGravity = -2f;

    private CharacterController controller;
    private Vector3 velocity;
    private bool isGrounded;

    // Dirección guardada cuando el jugador salta
    private Vector3 airMoveDirection;
    public bool canMove = true;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (!canMove) return; 
        
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = groundedGravity;
        }

        Vector3 move = Vector3.zero;

        // Solo tomamos input si estamos en el suelo
        if (isGrounded)
        {
            float x = Input.GetAxisRaw("Horizontal");
            float z = Input.GetAxisRaw("Vertical");

            move = transform.right * x + transform.forward * z;
            if (move.sqrMagnitude > 1f) move.Normalize();

            // Si hay movimiento, actualizamos la dirección de movimiento
            airMoveDirection = move;

            // Salto
            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
        }
        else
        {
            // En el aire usamos la dirección guardada (sin cambiarla)
            move = airMoveDirection;
        }

        // Movimiento horizontal
        controller.Move(move * moveSpeed * Time.deltaTime);

        // Gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}