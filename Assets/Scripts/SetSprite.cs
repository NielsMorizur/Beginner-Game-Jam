using UnityEngine;

public class SetSprite : MonoBehaviour
{
    GameManager gm;

    SpriteRenderer sr;

    public Sprite candleSprite;

    void Awake()
    {
        gm = GameManager.Instance;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (gm.completeCollected && sr.sprite != candleSprite)
        {
            sr.sprite = candleSprite;
        }
    }
}
