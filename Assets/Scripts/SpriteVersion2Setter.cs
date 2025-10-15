using UnityEngine;

public class SpriteVersion2Setter : MonoBehaviour
{
    private SpriteAnimator spriteScript;

    public Sprite idleSpriteUpper;
    public Sprite walkSprite1Upper;
    public Sprite walkSprite2Upper;
    public Sprite jumpSpriteUpper;

    private bool upperSwitch = false;

    void Awake()
    {
        spriteScript = GetComponent<SpriteAnimator>();
    }

    void Update()
    {
       if (GameManager.Instance.upperCollected && !upperSwitch)
        {
            upperSwitch = true;
            spriteScript.idleSprite = idleSpriteUpper;
            spriteScript.walkSprite1 = walkSprite1Upper;
            spriteScript.walkSprite2 = walkSprite2Upper;
            spriteScript.jumpSprite = jumpSpriteUpper;
        }
    }
}
