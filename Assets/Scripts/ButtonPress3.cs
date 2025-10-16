using UnityEngine;
using UnityEngine.Events;

public class ButtonPress3 : MonoBehaviour
{
    public Sprite offSprite;
    public Sprite onSprite;

    public bool revertOnExit = true;

    public string playerTag = "Player";

    private SpriteRenderer sr;

    public UnityEvent spawnCandles;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        if (offSprite != null)
            sr.sprite = offSprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            sr.sprite = onSprite;
            spawnCandles?.Invoke();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (revertOnExit && other.CompareTag(playerTag))
        {
            sr.sprite = offSprite;
        }
    }
}
