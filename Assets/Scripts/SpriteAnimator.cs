using UnityEngine;

public class SpriteAnimator : MonoBehaviour
{
    public Sprite idleSprite;
    public Sprite walkSprite1;
    public Sprite walkSprite2;
    public Sprite jumpSprite;

    public float frameRate = 6f; // how fast to swap walking frames (frames/sec)

    private SpriteRenderer sr;
    public Rigidbody2D rb;
    private float frameTimer;
    private bool toggleFrame;
    private bool wasMovingLastFrame;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.sprite = idleSprite;
    }

    void Update()
    {
        bool isGrounded = GetComponentInParent<PlayerMove>().IsGrounded();
        bool isMoving = Mathf.Abs(rb.linearVelocity.x) > 0.01f;

        if (!isGrounded)
        {
            // Show jump sprite if in air
            sr.sprite = jumpSprite;
            ResetWalkCycle();
        }
        else if (isMoving)
        {
            // Walking animation
            AnimateWalk();
        }
        else
        {
            // Idle
            sr.sprite = idleSprite;
            ResetWalkCycle();
        }
    }

    void AnimateWalk()
    {
        // If player just started moving, pick an immediate frame
        if (!wasMovingLastFrame)
        {
            toggleFrame = !toggleFrame;
            sr.sprite = toggleFrame ? walkSprite1 : walkSprite2;
            frameTimer = 0f;
        }

        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            toggleFrame = !toggleFrame;
            sr.sprite = toggleFrame ? walkSprite1 : walkSprite2;
        }

        wasMovingLastFrame = true;
    }

    void ResetWalkCycle()
    {
        frameTimer = 0f;
        wasMovingLastFrame = false;
    }
}