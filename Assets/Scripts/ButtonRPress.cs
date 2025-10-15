using UnityEngine;

public class ButtonRPress : MonoBehaviour
{
    public Sprite offSprite;
    public Sprite onSprite;

    public bool revertOnExit = true;

    public string friendTag = "Friend";

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
        if (collision.CompareTag(friendTag))
        {
            sr.sprite = onSprite;
            gm.OnButtonRPressed();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (revertOnExit && other.CompareTag(friendTag))
        {
            sr.sprite = offSprite;
            gm.OnButtonRReleased();
        }
    }
}
