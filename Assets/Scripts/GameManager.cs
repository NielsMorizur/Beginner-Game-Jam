using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool goalScored = false;
    
    public bool legsCollected = false;
    public bool upperCollected = false;
    public bool completeCollected = false;

    public bool buttonLPressed = false;
    public bool buttonRPressed = false;
    public bool bothButtonsPressed = false;

    public int candlesCollected = 0;
    public bool allCandlesCollected = false;

    public int trialCandles = 0;
    public bool trialSuccess = false;

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

    void Update()
    {
        if (buttonLPressed && buttonRPressed) bothButtonsPressed = true;
        if (candlesCollected + trialCandles >= 5) allCandlesCollected = true;
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

    public void OnCompleteCollected()
    {
        completeCollected = true;
    }

    public void OnButtonLPressed()
    {
        buttonLPressed = true;
    }

    public void OnButtonLReleased()
    {
        buttonLPressed = false;
    }

    public void OnButtonRReleased()
    {
        buttonRPressed = false;
    }

    public void OnButtonRPressed()
    {
        buttonRPressed = true;
    }

    public void OnCandleCollected()
    {
        candlesCollected += 1;
    }

    public void OnTrialCandleCollected()
    {
        trialCandles += 1;
        if (trialCandles >= 3) trialSuccess = true;
    }
}
