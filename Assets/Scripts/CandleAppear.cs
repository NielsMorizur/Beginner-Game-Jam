using UnityEngine;

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
        transform.position = ttm.candle1Location;
    }
    
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
