using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnBedLight : MonoBehaviour
{
    GameManager gm;
    
    void Awake()
    {
        gm = GameManager.Instance;
        gameObject.GetComponent<Light2D>().intensity = 0f;
    }

    void Update()
    {
        if (gm.completeCollected) gameObject.GetComponent<Light2D>().intensity = 0.33f;
    }
}
