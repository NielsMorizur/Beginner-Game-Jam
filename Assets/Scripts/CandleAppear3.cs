using UnityEngine;

public class CandleAppear3 : MonoBehaviour
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
        transform.position = ttm.candle3Location;
    }
    
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
