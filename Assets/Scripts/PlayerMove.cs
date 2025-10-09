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

    [SerializeField] float slopeStopThreshold = 0.1f;     // minimum |moveInput| to consider "moving"
    [SerializeField] float maxSlopeAngleToStop = 60f;     // only apply stop for slopes up to this angle
    [SerializeField] float groundCheckDistance = 0.17f;


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

        HandleSlopeStop();
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

    public bool IsGrounded()
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

    void HandleSlopeStop()
    {
        // If player is pressing (walking), don't stop
        if (Mathf.Abs(moveInput) > slopeStopThreshold) return;

        // Raycast down from the groundCheck point to detect ground and get the normal
        Vector2 origin = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position;
        float distance = groundCheckDistance; // slightly larger than your ground check radius
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, distance, groundLayer);

        if (!hit.collider) return; // not grounded according to this raycast

        // compute slope angle
        float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
        if (slopeAngle > maxSlopeAngleToStop) return; // too steep; maybe we want sliding

        // compute downhill direction (perpendicular to normal)
        // downhill = (normal.y, -normal.x) gives direction along slope pointing down
        Vector2 downhill = new Vector2(hit.normal.y, -hit.normal.x).normalized;

        // compute velocity component along downhill direction
        float downhillVel = Vector2.Dot(rb.linearVelocity, downhill);

        // If downhillVel is positive (meaning it's pointing downhill), remove that component
        if (downhillVel > 0.0001f)
        {
            Vector2 cancel = downhill * downhillVel;
            rb.linearVelocity = rb.linearVelocity - cancel;
            // Optionally zero out small y wiggle if necessary:
            // if (Mathf.Abs(rb.velocity.y) < 0.01f) rb.velocity = new Vector2(rb.velocity.x, 0f);
        }
    }
}