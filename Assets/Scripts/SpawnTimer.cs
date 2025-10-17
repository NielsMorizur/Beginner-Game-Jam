using UnityEngine;

public class SpawnTimer : MonoBehaviour
{
    void Awake()
    {
        gameObject.SetActive(false);
    }

    public void EnableTimer()
    {
        gameObject.SetActive(true);
    }
    
    public void DisableTimer()
    {
        gameObject.SetActive(false);
    }
}
