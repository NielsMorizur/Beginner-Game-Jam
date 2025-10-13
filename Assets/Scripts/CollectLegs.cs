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
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            gm.OnLegsCollected();
            legsCollected?.Invoke();
        }
    }
}
