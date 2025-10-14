using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool goalScored = false;
    public bool legsCollected = false;
    public bool upperCollected = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // keeps same GameManager when switching scenes
    }

    public void OnGoalScored()
    {
        goalScored = true;
    }

    public void OnLegsCollected()
    {
        legsCollected = true;
    }

    public void OnUpperCollected()
    {
        upperCollected = true;
    }
}
