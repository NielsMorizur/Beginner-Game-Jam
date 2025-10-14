using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor2 : MonoBehaviour
{
    public string levelToLoad = "UpperLevel";
    private bool playerInRange = false;

    GameManager gm;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && gm.legsCollected && !gm.upperCollected)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}