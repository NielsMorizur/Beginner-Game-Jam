using UnityEngine;
using UnityEngine.SceneManagement;

public class EnterBed : MonoBehaviour
{
    private bool playerInRange = false;

    GameObject player;

    GameManager gm;

    public EndGameController faderController;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            player = other.gameObject;
        }
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
            if (faderController != null)
                faderController.StartEndSequence(player, "Thank you for playing");
        }
    }
}