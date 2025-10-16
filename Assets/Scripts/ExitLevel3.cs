using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel3 : MonoBehaviour
{
    GameManager gm;

    public string levelToLoad = "MainScene";
    private bool playerInRange = false;

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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && gm.completeCollected)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
