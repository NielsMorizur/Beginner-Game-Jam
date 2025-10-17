using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnExitDoorLightLegs : MonoBehaviour
{
    GameManager gm;
    
    void Awake()
    {
        gm = GameManager.Instance;
        gameObject.GetComponent<Light2D>().enabled = false;
    }

    void Update()
    {
        if (gm.legsCollected) gameObject.GetComponent<Light2D>().enabled = true;
    }
}
