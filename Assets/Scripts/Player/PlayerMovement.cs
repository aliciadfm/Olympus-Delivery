using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class NewMonoBehaviourScript : MonoBehaviour
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

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0f) velocity.y = groundedGravity;

        // Input de movimiento (plano XZ)
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;
        if (move.sqrMagnitude > 1f) move.Normalize();

        controller.Move(move * moveSpeed * Time.deltaTime);


        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            // v = sqrt(2 * h * -g)
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }
}
