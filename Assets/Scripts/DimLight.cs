using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DimLight : MonoBehaviour
{
    public Light2D targetLight;
    public float fadeDuration = 2f;
    private bool isFading = false;
    private float fadeTimer;

    private float startIntensity;

    private void Awake()
    {
        if (targetLight == null)
            targetLight = GetComponent<Light2D>();

        startIntensity = targetLight.intensity;
    }

    public void StartFade()
    {
        if (!isFading)
        {
            fadeTimer = 0f;
            isFading = true;
        }
    }

    private void Update()
    {
        if (!isFading)
            return;

        fadeTimer += Time.deltaTime;
        float t = fadeTimer / fadeDuration;
        targetLight.intensity = Mathf.Lerp(startIntensity, 0f, t);

        if (t >= 1f)
        {
            targetLight.intensity = 0f;
            isFading = false;
        }

        if(GameManager.Instance.legsCollected)
        {
            enabled = false;
        }
    }
}
