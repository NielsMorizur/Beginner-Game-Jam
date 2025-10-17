using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SpawnBedLight : MonoBehaviour
{
    GameManager gm;
    
    void Awake()
    {
        gameObject.GetComponent<Light2D>().intensity = 0f;
        gm = GameManager.Instance;
    }

    void Update()
    {
        if (gm.completeCollected) gameObject.GetComponent<Light2D>().intensity = 0.33f;
    }
}
