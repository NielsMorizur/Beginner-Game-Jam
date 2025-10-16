using UnityEngine;
using UnityEngine.Events;

public class CollideWithFriend : MonoBehaviour
{
    public Rigidbody2D friendRb;
    public UnityEvent friendCollided;

    public GameObject friendObject;

    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            gm.upperCollected = true;
            friendObject.GetComponent<PlayerMove>().enabled = false;
            friendRb.constraints |= RigidbodyConstraints2D.FreezePosition;
            friendCollided?.Invoke();
            Invoke("delayMovementEnabled", 1.5f);
            friendObject.GetComponentInChildren<SpriteRenderer>().enabled = false;
            friendObject.GetComponent<Collider2D>().enabled = false;
            friendObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
        }
    }
    
    void delayMovementEnabled()
    {
        friendObject.GetComponent<PlayerMove>().enabled = true;
    }
}
