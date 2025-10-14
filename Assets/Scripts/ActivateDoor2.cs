using UnityEngine;

public class ActivateDoor2 : MonoBehaviour
{
    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (gm.legsCollected)
        {
            GetComponent<SpriteRenderer>().enabled = true;
        }
    }
}
