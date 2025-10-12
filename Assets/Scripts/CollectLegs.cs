using UnityEngine;
using UnityEngine.Events;

public class CollectLegs : MonoBehaviour
{
    public UnityEvent legsCollected;
    public string playerTag = "Player";

    private GameObject player;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            player = collision.gameObject;
            GetComponentInChildren<SpriteRenderer>().enabled = false;
            legsCollected?.Invoke();
        }
    }
}
