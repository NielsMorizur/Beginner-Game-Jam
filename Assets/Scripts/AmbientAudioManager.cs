using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbientAudioManager : MonoBehaviour
{
    public static AmbientAudioManager Instance { get; private set; }

    [Header("Clips")]
    [Tooltip("Plays once at startup (optional).")]
    public AudioClip startClip;
    [Tooltip("Plays repeatedly after the start clip finishes.")]
    public AudioClip loopClip;

    [Header("Volume")]
    [Range(0f,1f)]
    public float initialVolume = 0.6f;

    [Header("Behavior")]
    [Tooltip("If true, the manager will start playing automatically on Awake.")]
    public bool playOnAwakeSequence = true;

    AudioSource src;
    Coroutine fadeCoroutine;
    Coroutine startupCoroutine;
    //bool isLooping = false;
    bool sequenceStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        src = GetComponent<AudioSource>();
        src.loop = false; // we'll set loop manually when switching to loopClip
        src.playOnAwake = false;
        src.clip = null;
        src.volume = initialVolume;
    }

    void Start()
    {
        if (playOnAwakeSequence)
            PlayAmbientSequence();
    }

    /// <summary>
    /// Starts the start->loop sequence. Safe to call multiple times.
    /// </summary>
    public void PlayAmbientSequence()
    {
        if (sequenceStarted) return;
        sequenceStarted = true;

        // If a startup clip exists, play it and then start the loop; otherwise start loop immediately.
        if (startClip != null)
        {
            if (startupCoroutine != null) StopCoroutine(startupCoroutine);
            startupCoroutine = StartCoroutine(PlayStartThenLoop());
        }
        else
        {
            StartLoopClipImmediate();
        }
    }

    IEnumerator PlayStartThenLoop()
    {
        //isLooping = false;

        // Play the start clip
        src.clip = startClip;
        src.loop = false;
        if (!src.isPlaying) src.Play();

        // Wait until the start clip finishes (use unscaled time)
        float remaining = startClip.length;
        float t = 0f;
        while (t < remaining)
        {
            t += Time.unscaledDeltaTime;
            yield return null;
        }

        // Switch to loop clip
        StartLoopClipImmediate();
        startupCoroutine = null;
    }

    void StartLoopClipImmediate()
    {
        if (loopClip == null)
        {
            // nothing to loop; just return
            return;
        }

        //isLooping = true;
        src.clip = loopClip;
        src.loop = true;
        if (!src.isPlaying) src.Play();
    }

    /// <summary>
    /// Fade the ambient audio down to target volume over duration (unscaled time).
    /// If stopAfter is true and targetVolume is ~0, the AudioSource will Stop() when done.
    /// </summary>
    public void FadeToVolume(float targetVolume, float duration = 1f, bool stopAfter = false)
    {
        if (fadeCoroutine != null) StopCoroutine(fadeCoroutine);
        fadeCoroutine = StartCoroutine(FadeTo(targetVolume, duration, stopAfter));
    }

    public void FadeOut(float duration = 1.5f, bool stopAfter = true)
    {
        FadeToVolume(0f, duration, stopAfter);
    }

    IEnumerator FadeTo(float targetVol, float duration, bool stopAfter)
    {
        if (src == null) yield break;

        float start = src.volume;
        if (duration <= 0f)
        {
            src.volume = targetVol;
            if (stopAfter && Mathf.Approximately(targetVol, 0f)) src.Stop();
            fadeCoroutine = null;
            yield break;
        }

        float t = 0f;
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / duration);
            src.volume = Mathf.Lerp(start, targetVol, 1f - Mathf.Pow(1f - u, 2f)); // ease-out
            yield return null;
        }

        src.volume = targetVol;
        if (stopAfter && Mathf.Approximately(targetVol, 0f)) src.Stop();
        fadeCoroutine = null;
    }

    /// <summary>
    /// Immediately stop ambient and reset state if you need to restart later.
    /// </summary>
    public void StopAmbientImmediate(bool resetSequence = false)
    {
        if (startupCoroutine != null) { StopCoroutine(startupCoroutine); startupCoroutine = null; }
        if (fadeCoroutine != null) { StopCoroutine(fadeCoroutine); fadeCoroutine = null; }
        src.Stop();
        src.volume = 0f;
        //isLooping = false;
        if (resetSequence) sequenceStarted = false;
    }

    /// <summary>
    /// Force restart the whole start->loop sequence (stops whatever is playing first).
    /// </summary>
    public void RestartSequence()
    {
        StopAmbientImmediate(resetSequence: true);
        src.volume = initialVolume;
        PlayAmbientSequence();
    }
}
