using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnExitDoorLightComplete : MonoBehaviour
{
    GameManager gm;
    
    void Awake()
    {
        gm = GameManager.Instance;
        gameObject.GetComponent<Light2D>().enabled = false;
    }

    void Update()
    {
        if (gm.completeCollected) gameObject.GetComponent<Light2D>().enabled = true;
    }
}
