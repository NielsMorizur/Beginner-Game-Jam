using UnityEngine;
using UnityEngine.Events;

public class TimeTrialManager : MonoBehaviour
{
    public float timeLimit = 7f;

    public float timeLeft;

    public UnityEvent timeUp;
    public UnityEvent trialSuccess;

    public Vector3 candle3Location = new Vector3(0f, 2.52f, 0f);
    public Vector3 candle2Location = new Vector3(-7.21f, 1.57f, 0f);
    public Vector3 candle1Location = new Vector3(6.17f, 1.53f, 0f);

    GameManager gm;

    Coroutine runningTimer = null;

    void Awake()
    {
        gm = GameManager.Instance;
    }

    public void StartTimer()
    {
        gm.trialCandles = 0;
        timeLeft = timeLimit;

        if (runningTimer != null)
        {
            StopCoroutine(runningTimer);
            runningTimer = null;
        }

        foreach (var mover in FindObjectsByType<SinusoidalMover>(FindObjectsSortMode.None))
        {
            mover.ResetPosition();
        }

        runningTimer = StartCoroutine(Timer());
    }

    System.Collections.IEnumerator Timer()
    {
        while (timeLeft > 0f)
        {
            timeLeft -= Time.deltaTime;
            if (gm.trialSuccess)
            {
                Debug.Log("success");
                trialSuccess?.Invoke();
                yield break;
            }
            yield return null;
        }
        gm.trialCandles = 0;
        Debug.Log("fail");
        runningTimer = null;
        timeUp?.Invoke();
    }
}
