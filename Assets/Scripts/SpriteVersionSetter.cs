using UnityEngine;

public class SpriteVersionSetter : MonoBehaviour
{
    private SpriteAnimator spriteScript;

    public Sprite idleSpriteLegs;
    public Sprite walkSprite1Legs;
    public Sprite walkSprite2Legs;
    public Sprite jumpSpriteLegs;

    public Sprite idleSpriteUpper;
    public Sprite walkSprite1Upper;
    public Sprite walkSprite2Upper;
    public Sprite jumpSpriteUpper;

    public Sprite idleSpriteComplete;
    public Sprite walkSprite1Complete;
    public Sprite walkSprite2Complete;
    public Sprite jumpSpriteComplete;

    private bool legsSwitch = false;
    private bool upperSwitch = false;
    private bool completeSwitch = false;

    void Awake()
    {
        spriteScript = GetComponent<SpriteAnimator>();
    }

    void Update()
    {
        if (GameManager.Instance.legsCollected && !legsSwitch)
        {
            legsSwitch = true;
            spriteScript.idleSprite = idleSpriteLegs;
            spriteScript.walkSprite1 = walkSprite1Legs;
            spriteScript.walkSprite2 = walkSprite2Legs;
            spriteScript.jumpSprite = jumpSpriteLegs;
        }
        if (GameManager.Instance.upperCollected && !upperSwitch)
        {
            upperSwitch = true;
            spriteScript.idleSprite = idleSpriteUpper;
            spriteScript.walkSprite1 = walkSprite1Upper;
            spriteScript.walkSprite2 = walkSprite2Upper;
            spriteScript.jumpSprite = jumpSpriteUpper;
        }
        if (GameManager.Instance.completeCollected && !completeSwitch)
        {
            completeSwitch = true;
            spriteScript.idleSprite = idleSpriteComplete;
            spriteScript.walkSprite1 = walkSprite1Complete;
            spriteScript.walkSprite2 = walkSprite2Complete;
            spriteScript.jumpSprite = jumpSpriteComplete;
        }
    }
}
