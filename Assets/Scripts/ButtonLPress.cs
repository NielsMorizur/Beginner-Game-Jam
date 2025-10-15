using UnityEngine;

public class ButtonLPress : MonoBehaviour
{
    public Sprite offSprite;
    public Sprite onSprite;

    public bool revertOnExit = true;

    public string playerTag = "Player";

    private SpriteRenderer sr;

    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;

        sr = GetComponent<SpriteRenderer>();
        if (offSprite != null)
            sr.sprite = offSprite;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(playerTag))
        {
            sr.sprite = onSprite;
            gm.OnButtonLPressed();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (revertOnExit && other.CompareTag(playerTag))
        {
            sr.sprite = offSprite;
            gm.OnButtonLReleased();
        }
    }
}
