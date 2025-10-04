using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMove : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference moveAction;
    public InputActionReference jumpAction;

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Jump")]
    public float jumpForce = 12f;
    public float coyoteTime = 0.12f;      // grace time after leaving ground
    public float jumpBufferTime = 0.12f;  // buffer time before landing to accept jump input
    public float shortHopMultiplier = 0.5f; // multiply y velocity on early jump release

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.16f;
    public LayerMask groundLayer;

    [Header("Sprite / Facing")]
    public bool flipSpriteByScale = true; // flips localScale.x based on movement

    Rigidbody2D rb;

    // runtime
    float moveInput = 0f;
    bool wantJump = false;
    bool jumpHeld = false;
    float lastGroundedTime = -999f;
    float lastJumpPressedTime = -999f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null) moveAction.action.Enable();
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.Enable();
            jumpAction.action.performed += OnJumpPerformed;
            jumpAction.action.canceled += OnJumpCanceled;
        }
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null) moveAction.action.Disable();
        if (jumpAction != null && jumpAction.action != null)
        {
            jumpAction.action.performed -= OnJumpPerformed;
            jumpAction.action.canceled -= OnJumpCanceled;
            jumpAction.action.Disable();
        }
    }

    void Update()
    {
        // Read horizontal input (Vector2.x)
        if (moveAction != null && moveAction.action != null)
        {
            Vector2 v = moveAction.action.ReadValue<Vector2>();
            moveInput = v.x;
        }
        else
        {
            moveInput = 0f;
        }

        // track jump pressed time for buffering
        if (wantJump)
        {
            // already set by callback; keep the last-press timestamp
            lastJumpPressedTime = Time.time;
            wantJump = false;
        }

        // track if jump is held (for variable jump)
        jumpHeld = jumpAction != null && jumpAction.action != null && jumpAction.action.ReadValue<float>() > 0.5f;

        // update last grounded time (coyote time)
        bool grounded = IsGrounded();
        if (grounded)
        {
            lastGroundedTime = Time.time;
        }

        // Flip sprite if moving
        if (flipSpriteByScale && Mathf.Abs(moveInput) > 0.01f)
        {
            Vector3 s = transform.localScale;
            s.x = Mathf.Sign(moveInput) * Mathf.Abs(s.x);
            transform.localScale = s;
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);

        // Jump buffering + coyote time
        bool canUseCoyote = (Time.time - lastGroundedTime) <= coyoteTime;
        bool hasJumpBuffered = (Time.time - lastJumpPressedTime) <= jumpBufferTime;

        if (hasJumpBuffered && canUseCoyote)
        {
            PerformJump();
            lastJumpPressedTime = -999f;
        }

        // variable jump: if jump was released early while rising
        if (!jumpHeld && rb.linearVelocity.y > 0.01f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * shortHopMultiplier);
        }
    }

    void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        // called when jump is pressed
        // set a flag/time for buffering
        wantJump = true;
    }

    void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        // handled in FixedUpdate using jumpHeld
        // leaving here to keep symmetry / future expansion
    }

    void PerformJump()
    {
        // set vertical velocity directly for crisp jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    bool IsGrounded()
    {
        if (groundCheck == null) return false;
        Collider2D hit = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return hit != null;
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
        }
    }
}
