using UnityEngine;
using UnityEngine.Events;

public class CollectLegs : MonoBehaviour
{
    public UnityEvent legsCollected;
    public string playerTag = "Player";

    private GameObject player;
    
    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag) && !gm.legsCollected)
        {
            player = collision.gameObject;
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            gm.OnLegsCollected();
            legsCollected?.Invoke();
            Invoke("delayMovementEnabled", 1.5f);
        }
    }

    void delayMovementEnabled()
    {
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
