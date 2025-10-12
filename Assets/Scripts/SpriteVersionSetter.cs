using UnityEngine;

public class SpriteVersionSetter : MonoBehaviour
{
    private SpriteAnimator spriteScript;

    public Sprite idleSpriteLegs;
    public Sprite walkSprite1Legs;
    public Sprite walkSprite2Legs;
    public Sprite jumpSpriteLegs;

    private bool legsSwitch = false;

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
    }
}
