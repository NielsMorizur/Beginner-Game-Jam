using UnityEngine;
using UnityEngine.Rendering.Universal;
using System.Collections;
using UnityEngine.Events;

public class LightFlash3 : MonoBehaviour
{
    public Light2D light2d;
    public float peakIntensity = 2f;
    public float peakRadius = 2f;
    public float growDuration = 0.15f;
    public float holdDuration = 0.08f;
    public float fadeDuration = 0.7f;

    private float initialIntensity = 0.4f;
    private Vector3 initialScale = new Vector3(1f, 1f, 1f);

    [System.Serializable] public class FloatEvent : UnityEvent<float> {}
    public FloatEvent onIntensityChanged;
    
    void Awake()
    {
        if (light2d == null) light2d = GetComponent<Light2D>();
    }

    // void Start()
    // {
    //     ResetLight();
    // }

    public void ResetLight()
    {
        light2d.enabled = true;
        light2d.intensity = initialIntensity;
        transform.localScale = initialScale;
        onIntensityChanged?.Invoke(1f);
    }

    public void Play()
    {
        StopAllCoroutines();
        StartCoroutine(PlayRoutine());
    }

    IEnumerator PlayRoutine()
    {
        float t = 0f;

        // Grow: from 0 -> peak
        t = 0f;
        while (t < growDuration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / growDuration);
            // ease out for snappy pop: use second-order ease
            float ease = 1f - Mathf.Pow(1f - u, 3f);
            light2d.intensity = Mathf.Lerp(0f, peakIntensity, ease);
            float size = Mathf.Lerp(0f, peakRadius, ease);
            transform.localScale = new Vector3(size, size, size);

            // float normalized = (peakIntensity == 0f) ? 0f : Mathf.Clamp01(light2d.intensity / peakIntensity);
            // onIntensityChanged?.Invoke(normalized);

            yield return null;
        }

        // hold briefly
        light2d.intensity = peakIntensity;
        light2d.pointLightOuterRadius = peakRadius;
        // onIntensityChanged?.Invoke(1f);
        yield return new WaitForSecondsRealtime(holdDuration);

        // fade to 0
        t = 0f;
        float startIntensity = light2d.intensity;
        float startRadius = transform.localScale.x;
        while (t < fadeDuration)
        {
            t += Time.unscaledDeltaTime;
            float u = Mathf.Clamp01(t / fadeDuration);
            float ease = 1f - Mathf.Pow(1f - u, 2f); // ease-out
            light2d.intensity = Mathf.Lerp(startIntensity, 0f, ease);
            float size = Mathf.Lerp(startRadius, 0f, ease);
            transform.localScale = new Vector3(size, size, size);

            float normalized = (peakIntensity == 0f) ? 0f : Mathf.Clamp01(light2d.intensity / peakIntensity);
            onIntensityChanged?.Invoke(normalized);

            yield return null;
        }

        onIntensityChanged?.Invoke(0f);
        light2d.enabled = false;
    }
}
