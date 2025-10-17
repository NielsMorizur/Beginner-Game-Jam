using UnityEngine;

public class TimeTrialAppear : MonoBehaviour
{
    public void EnableSelf()
    {
        gameObject.SetActive(true);
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<BoxCollider2D>().enabled = true;
    }
    
    public void DisableSelf()
    {
        gameObject.SetActive(false);
    }
}
