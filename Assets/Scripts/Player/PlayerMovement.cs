using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")] public float speed = 5f;
    public float jumpForce = 6f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private bool isGrounded = true;

    private AnimacionesPlayer animacionesPlayer;


    private bool canMove = true;

    [SerializeField] public float runMultiplier = 2f;

    [SerializeField] private bool isRunning = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animacionesPlayer = GetComponent<AnimacionesPlayer>();
    }



    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    // ===== INPUT SYSTEM =====
    // Estas funciones deben coincidir con los nombres de las acciones del Input System
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnJump(InputValue value)
    {
        if (value.isPressed)
            if (isGrounded)
                animacionesPlayer.AnimacionSaltar01();
    }

    public void OnGolpear(InputValue value)
    {
        if (value.isPressed)
            if (isGrounded)
                animacionesPlayer.Golpear();
    }

    public void Saltar()
    {
        animacionesPlayer.AnimacionSaltar02();
        rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    // ===== MOVIMIENTO =====
    void FixedUpdate()
    {
        if (!canMove) return;
        // Movimiento en plano X/Z
        ////////////////////////////////////////////////////////////////////////////// cambiada
        // Vector3 direction = new Vector3(moveInput.x, 0, moveInput.y);
        // por esta

        // 1. Detectamos si CTRL está pulsado AHORA MISMO
        bool ctrlPressed = Keyboard.current != null &&
                           (Keyboard.current.leftCtrlKey.isPressed ||
                            Keyboard.current.rightCtrlKey.isPressed);

        isRunning = ctrlPressed;  

        Vector3 direction = transform.TransformDirection(new Vector3(moveInput.x, 0, moveInput.y));
////Run
        float currentSpeed = isRunning ? speed * runMultiplier : speed;
        Vector3 velocity = direction * currentSpeed;
        //    Vector3 velocity = direction * speed;
        Vector3 newVelocity = new Vector3(velocity.x, rb.linearVelocity.y, velocity.z);

        rb.linearVelocity = newVelocity;
    }

    // Detectar suelo
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            animacionesPlayer.Ensuelo(true);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
            animacionesPlayer.Ensuelo(false);
        }
    }


    void OnEnable()
    {
        moveInput = Vector2.zero;
    }
}