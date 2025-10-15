using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitLevel2 : MonoBehaviour
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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && gm.upperCollected)
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
