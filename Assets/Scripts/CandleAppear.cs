using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CandleAppear : MonoBehaviour
{
    TimeTrialManager ttm;

    void Awake()
    {
        ttm = FindAnyObjectByType<TimeTrialManager>();
    }

    public void EnableSelf()
    {
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<SinusoidalMover>().enabled = true;
        GetComponentInChildren<Light2D>().intensity = 0.4f;
        transform.position = ttm.candle1Location;
    }
    
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
