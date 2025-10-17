using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameController : MonoBehaviour
{
    [Header("UI References")]
    public Image blackImage;

    public TMP_Text messageText;

    [Header("Fade Timings")]
    public float startFadeDuration = 3f;
    public float endFadeDuration = 3f;
    public float messageDelay = 0.8f;
    public float messageFadeDuration = 0.8f;
    public bool useUnscaledTime = true;

    [Header("Ambient Audio")]
    public bool autoPlayAmbient = true;
    [Tooltip("Ambient fade amount will be driven to AmbientAudioManager.initialVolume at start")]
    public float ambientFadeInDuration = 1.5f;
    public float ambientFadeOutDuration = 1.5f;

    [Header("Behavior")]
    public bool disablePlayerDuringEnd = true;

    bool isRunning = false;
    Coroutine runningFade = null;

    static bool s_hasDoneInitialFade = false;
    private bool hasDoneEndFade = false;

    void Awake()
    {
        if (blackImage == null)
        {
            Debug.LogError("[EndGameController] blackImage not assigned.");
            enabled = false;
            return;
        }
        if (!s_hasDoneInitialFade)
        {
            // Ensure blackImage active and start fully black if we plan to fade-in
            blackImage.gameObject.SetActive(true);
            SetBlackAlpha(1f);
        }
        // Hide message initially
        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            SetMessageAlpha(0f);
        }
    }

    void Start()
    {
        if (!s_hasDoneInitialFade)
        {
            s_hasDoneInitialFade = true;
            // Start ambient and fade-in
            if (autoPlayAmbient && AmbientAudioManager.Instance != null)
            {
                AmbientAudioManager.Instance.PlayAmbientSequence();
                // If ambient is set to initialVolume inside manager, we can fade from 0 -> initialVolume
                // But to be safe, fade to its initialVolume property if present.
                AmbientAudioManager.Instance.FadeToVolume(AmbientAudioManager.Instance != null ? AmbientAudioManager.Instance.initialVolume : 1f, ambientFadeInDuration, stopAfter: false);
            }

            // Fade from black -> transparent at start
            FindAnyObjectByType<PlayerMove>().enabled = false;
            FadeIn(startFadeDuration, null);
            FindAnyObjectByType<PlayerMove>().enabled = true;
        }
    }

    // Public: fade black -> transparent
    public void FadeIn(float duration = 1f, System.Action onComplete = null)
    {
        // stop any end sequence if running
        if (runningFade != null) StopCoroutine(runningFade);
        runningFade = StartCoroutine(FadeRoutine(from: blackImage.color.a, to: 0f, duration: duration, onComplete: onComplete));
    }

    // Public: fade transparent -> black and show message; disables player input while running
    public void StartEndSequence(GameObject player = null, string message = "Thank you for playing")
    {
        if (isRunning || hasDoneEndFade) return;
        hasDoneEndFade = true;
        blackImage.gameObject.SetActive(true);
        SetBlackAlpha(0f);
        StartCoroutine(EndSequenceRoutine(player, message));
    }

    IEnumerator EndSequenceRoutine(GameObject player, string message)
    {
        isRunning = true;

        // optionally disable player input components
        if (disablePlayerDuringEnd && player != null)
        {
            var pm = player.GetComponent<MonoBehaviour>(); // placeholder to avoid compile error
            var playerMove = player.GetComponent("PlayerMove") as MonoBehaviour;
            var playerInput = player.GetComponent("PlayerInput") as MonoBehaviour;
            if (playerMove != null) playerMove.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
        }

        // start ambient fade-out (match visual fade time)
        if (AmbientAudioManager.Instance != null)
        {
            AmbientAudioManager.Instance.FadeOut(endFadeDuration > 0f ? endFadeDuration : ambientFadeOutDuration, stopAfter: true);
        }

        // Visual fade to black
        if (runningFade != null) StopCoroutine(runningFade);
        runningFade = StartCoroutine(FadeRoutine(from: blackImage.color.a, to: 1f, duration: endFadeDuration, onComplete: null));
        // wait for the visual fade to finish
        float wait = endFadeDuration;
        while (wait > 0f)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            wait -= dt;
            yield return null;
        }

        // delay before message
        float delay = messageDelay;
        while (delay > 0f)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            delay -= dt;
            yield return null;
        }

        // show message
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);

            // fade in text
            if (messageFadeDuration <= 0f)
            {
                SetMessageAlpha(1f);
            }
            else
            {
                float t = 0f;
                while (t < messageFadeDuration)
                {
                    float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    t += dt;
                    float u = Mathf.Clamp01(t / messageFadeDuration);
                    SetMessageAlpha(u);
                    yield return null;
                }
                SetMessageAlpha(1f);
            }
        }

        // optionally re-enable some behaviours after end (we leave player disabled intentionally)
        isRunning = false;
    }

    // core fade coroutine used by both start and end
    IEnumerator FadeRoutine(float from, float to, float duration, System.Action onComplete)
    {
        float t = 0f;
        SetBlackAlpha(from);

        if (duration <= 0f)
        {
            SetBlackAlpha(to);
            onComplete?.Invoke();
            yield break;
        }

        while (t < duration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float u = Mathf.Clamp01(t / duration);
            float eased = Mathf.SmoothStep(from, to, u);
            SetBlackAlpha(eased);
            yield return null;
        }

        SetBlackAlpha(to);
        onComplete?.Invoke();
    }

    void SetBlackAlpha(float a)
    {
        if (blackImage == null) return;
        Color c = blackImage.color;
        c.a = Mathf.Clamp01(a);
        blackImage.color = c;
    }

    void SetMessageAlpha(float a)
    {
        if (messageText == null) return;
        var c = messageText.color;
        c.a = a;
        messageText.color = c;
    }
}
