using UnityEngine;
                                    
[RequireComponent(typeof(Rigidbody2D))]
public class PushableBox : MonoBehaviour
{
    public LayerMask playerLayer;           // set to the Player layer in Inspector
    public float pushMultiplier = 1.2f;     // how strongly the box reacts to player's push
    public float minPlayerPushSpeed = 0.1f; // minimum player's horizontal speed to count as pushing
    public float allowedNormalAngle = 60f;  // degrees: allowed angle between contact normal and up (used for side detection)

    Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        // quick filter: only react to Player layer
        if (((1 << collision.gameObject.layer) & playerLayer.value) == 0) return;

        // try to get player's rigidbody (if present)
        Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
        if (playerRb == null) return;

        // check player's horizontal velocity (you may also read player input component instead)
        float playerHorizVel = playerRb.linearVelocity.x;

        // we only push if player is moving into the box enough
        if (Mathf.Abs(playerHorizVel) < minPlayerPushSpeed) return;

        // examine contact points to determine which side of the box the player is contacting
        foreach (ContactPoint2D contact in collision.contacts)
        {
            // contact.normal points from the box surface toward the other collider (i.e. roughly the normal of the surface on the box)
            Vector2 normal = contact.normal.normalized;

            // compute dot with horizontal axis to see if contact is on left or right side
            // if player is to the left and pushing right, normal will point left (x < 0)
            // to allow only pushing horizontally, ensure normal is roughly horizontal:
            float horizontalAbs = Mathf.Abs(normal.x);
            float verticalAbs = Mathf.Abs(normal.y);

            // require that the contact is more horizontal than vertical (adjust threshold via allowedNormalAngle)
            // convert allowedNormalAngle to a minimum horizontal ratio:
            float minHorizontalRatio = Mathf.Cos(allowedNormalAngle * Mathf.Deg2Rad); // e.g. 60 deg -> 0.5
            // But horizontalAbs is already between 0..1, so require horizontalAbs >= minHorizontalRatio
            if (horizontalAbs < minHorizontalRatio) continue; // contact too vertical (e.g., top/bottom hit)

            // determine push direction: if player's velocity.x > 0, want to push to the right
            float direction = Mathf.Sign(playerHorizVel);

            // if contact normal.x and player's push direction line up correctly:
            // normal.x points from box toward player, so if the player is on the left pushing right, normal.x < 0
            if (direction > 0 && normal.x < 0 || direction < 0 && normal.x > 0)
            {
                // apply force proportional to player's horizontal velocity
                Vector2 pushForce = new Vector2(playerHorizVel * pushMultiplier, 0f);
                // Use AddForce for smooth acceleration, or modify velocity directly for snappier movement:
                rb.AddForce(pushForce, ForceMode2D.Force);
                // Optionally clamp the box velocity to prevent runaway:
                // rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxSpeed, maxSpeed), rb.velocity.y);
                break; // we've applied push for one contact, no need to test others
            }
        }
    }
}
