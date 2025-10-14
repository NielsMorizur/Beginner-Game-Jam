using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterDoor : MonoBehaviour
{
    public string levelToLoad = "LegsLevel";
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
        if (playerInRange && Input.GetKeyDown(KeyCode.E) && !gm.legsCollected)
        {
            SceneManager.LoadScene(levelToLoad);
        }

        if (gm.legsCollected)
        {
            Color newColor;
            ColorUtility.TryParseHtmlString("#E5E5E5", out newColor);
            GetComponent<SpriteRenderer>().color = newColor;
        }
    }
}