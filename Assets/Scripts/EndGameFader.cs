using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndGameFader : MonoBehaviour
{
    [Header("References (assign in Inspector)")]
    public Image blackImage;           // fullscreen black Image (alpha will be animated)

    public TMP_Text messageText;       // optional TextMeshProUGUI (set inactive initially)

    public TMP_Text instructionsText;

    [Header("Timing")]
    public float fadeDuration = 1.5f;  // seconds to fade to full black
    public float messageDelay = 0.4f;  // delay after black before showing message
    public float messageFadeDuration = 0.8f; // fade-in time for message (0 = instant)

    [Header("Behavior")]
    public bool useUnscaledTime = true; // fade while Time.timeScale == 0
    public bool disablePlayerDuringFade = true;

    // runtime
    bool isRunning = false;

    public bool hasFaded = false;

    void Awake()
    {
        if (blackImage == null)
            Debug.LogError("[EndGameFader] Assign a fullscreen black Image in the Inspector.");

        // Ensure initial state: blackImage transparent and message hidden
        if (blackImage != null)
        {
            Color c = blackImage.color;
            c.a = 0f;
            blackImage.color = c;
            blackImage.gameObject.SetActive(true);
        }

        if (messageText != null)
        {
            messageText.gameObject.SetActive(false);
            SetTextAlpha(0f);
        }
    }

    /// <summary>
    /// Call this to start the fade -> show message sequence.
    /// Optionally pass the player's GameObject so we can disable PlayerMove component.
    /// </summary>
    public void StartEndSequence(GameObject player = null, string message = "Thank you for playing")
    {
        if (isRunning || hasFaded) return;
        hasFaded = true;
        StartCoroutine(EndRoutine(player, message));
    }

    IEnumerator EndRoutine(GameObject player, string message)
    {
        isRunning = true;

        instructionsText.gameObject.SetActive(false);

        // disable player input (optional)
        if (disablePlayerDuringFade && player != null)
        {
            var pm = player.GetComponent<MonoBehaviour>(); // generic cast below
            // try common component names
            var playerMove = player.GetComponent("PlayerMove") as MonoBehaviour;
            var playerInput = player.GetComponent("PlayerInput") as MonoBehaviour;

            if (playerMove != null) playerMove.enabled = false;
            if (playerInput != null) playerInput.enabled = false;
        }

        // Fade blackImage alpha from 0 -> 1
        float t = 0f;
        while (t < fadeDuration)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            t += dt;
            float u = Mathf.Clamp01(t / fadeDuration);
            float eased = Mathf.SmoothStep(0f, 1f, u); // smooth easing
            SetBlackAlpha(eased);
            yield return null;
        }
        SetBlackAlpha(1f);

        // small delay
        float delay = messageDelay;
        while (delay > 0f)
        {
            float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
            delay -= dt;
            yield return null;
        }

        // Show message
        if (messageText != null)
        {
            messageText.text = message;
            messageText.gameObject.SetActive(true);

            if (messageFadeDuration <= 0f)
            {
                SetTextAlpha(1f);
            }
            else
            {
                t = 0f;
                while (t < messageFadeDuration)
                {
                    float dt = useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;
                    t += dt;
                    float u = Mathf.Clamp01(t / messageFadeDuration);
                    SetTextAlpha(u);
                    yield return null;
                }
                SetTextAlpha(1f);
            }
        }

        // optionally keep it onscreen; or you can auto-quit/return to menu after delay
        isRunning = false;
    }

    void SetBlackAlpha(float a)
    {
        if (blackImage == null) return;
        Color c = blackImage.color;
        c.a = Mathf.Clamp01(a);
        blackImage.color = c;
    }

    void SetTextAlpha(float a)
    {
        if (messageText == null) return;
#if USING_TMPRO
        var c = messageText.color;
        c.a = a;
        messageText.color = c;
#else
        var c = messageText.color;
        c.a = a;
        messageText.color = c;
#endif
    }
}
