using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SpawnHead : MonoBehaviour
{
    GameManager gm;

    public UnityEvent completeCollected;

    private GameObject player;

    private bool headSpawned = false;
    private bool headCollected = false;

    void Awake()
    {
        gm = GameManager.Instance;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CircleCollider2D>().enabled = false;
        gameObject.GetComponentInChildren<Light2D>().enabled = false;
    }

    void Update()
    {
        if (gm.allCandlesCollected && !headSpawned)
        {
            headSpawned = true;
            gameObject.GetComponent<SpriteRenderer>().enabled = true;
            gameObject.GetComponent<CircleCollider2D>().enabled = true;
            gameObject.GetComponentInChildren<Light2D>().enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && gm.allCandlesCollected && !headCollected)
        {
            headCollected = true;
            player = collision.gameObject;
            player.GetComponent<PlayerMove>().enabled = false;
            player.GetComponent<Rigidbody2D>().constraints |= RigidbodyConstraints2D.FreezePositionX;
            gameObject.GetComponent<SpriteRenderer>().enabled = false;
            gm.OnCompleteCollected();
            completeCollected?.Invoke();
            Invoke("delayMovementEnabled", 1.5f);
        }
    }
    
    void delayMovementEnabled()
    {
        gameObject.SetActive(false);
        player.GetComponent<PlayerMove>().enabled = true;
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
    }
}
