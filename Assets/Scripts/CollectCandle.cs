using UnityEngine;
using UnityEngine.Events;

public class CollectCandle : MonoBehaviour
{
    GameManager gm;
    public UnityEvent candleTaken;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gm.OnCandleCollected();
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            candleTaken?.Invoke();
        }
    }
}
