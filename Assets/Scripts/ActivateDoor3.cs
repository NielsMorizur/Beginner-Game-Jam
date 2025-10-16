using UnityEngine;

public class ActivateDoor3 : MonoBehaviour
{
    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (gm.upperCollected)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
