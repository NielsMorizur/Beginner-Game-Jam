using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnExitDoorLightUpper : MonoBehaviour
{
    GameManager gm;
    
    void Awake()
    {
        gm = GameManager.Instance;
        gameObject.GetComponent<Light2D>().enabled = false;
    }

    void Update()
    {
        if (gm.upperCollected) gameObject.GetComponent<Light2D>().enabled = true;
    }
}
