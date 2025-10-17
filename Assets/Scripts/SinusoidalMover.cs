using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class SinusoidalMover : MonoBehaviour
{
    public enum Axis { Horizontal, Vertical }

    [Header("Motion")]
    public Axis axis = Axis.Horizontal;
    public float amplitude = 2f;        // total half-distance from center
    public float speed = 1f;            // cycles per second (frequency)
    [Range(0f, Mathf.PI * 2f)]
    public float phase = 0f;            // starting phase offset

    [Header("Options")]
    public bool startAtOffset = true;   // if true, start at center + current transform offset
    public Vector2 additionalOffset;   // add a constant offset to the motion (local space)

    // Internal
    Vector2 startWorldPos;
    Rigidbody2D rb;
    float elapsed = 0f;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // Ensure physics-friendly settings
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;

        // record starting world pos
        startWorldPos = transform.position;
        if (startAtOffset)
        {
            // keep whatever transform position is set in the editor as center
            startWorldPos = transform.position;
        }
    }

    void FixedUpdate()
    {
        elapsed += Time.fixedDeltaTime;

        // sine value from -1..1
        float sine = Mathf.Sin((elapsed * speed * Mathf.PI * 2f) + phase);

        Vector2 offset;
        if (axis == Axis.Horizontal)
            offset = new Vector2(sine * amplitude, 0f);
        else
            offset = new Vector2(0f, sine * amplitude);

        // apply local additional offset (rotated by transform)
        Vector3 worldAdditional = transform.TransformVector(additionalOffset);

        Vector2 target = startWorldPos + offset + (Vector2)worldAdditional;

        rb.MovePosition(target);
    }

    public void ResetPosition()
    {
        elapsed = 0f;
        rb.MovePosition(startWorldPos);
        transform.position = startWorldPos;
    }

    // Optional: draw gizmo in editor for visualizing path
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 center = Application.isPlaying ? startWorldPos : transform.position;
        if (!Application.isPlaying && startAtOffset) center = transform.position;
        if (axis == Axis.Horizontal)
        {
            Gizmos.DrawLine(center + Vector3.left * amplitude, center + Vector3.right * amplitude);
        }
        else
        {
            Gizmos.DrawLine(center + Vector3.down * amplitude, center + Vector3.up * amplitude);
        }
    }
}
