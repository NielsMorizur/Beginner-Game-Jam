using UnityEngine;

public class LightResponder : MonoBehaviour
{
    public bool subscribeAutomatically = true;
    public LegsCollect lightDimmer;
    public Color darkColor = Color.black;
    [Range(0f,1f)] public float darkenAmount = 1f;

    private SpriteRenderer sr;
    private Color originalColor;

    void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        originalColor = sr.color;
    }

    void OnEnable()
    {
        if (lightDimmer != null)
            lightDimmer.onIntensityChanged.AddListener(HandleIntensityChanged);
    }

    void OnDisable()
    {
        if (lightDimmer != null)
            lightDimmer.onIntensityChanged.RemoveListener(HandleIntensityChanged);
    }

    // intensity is normalized 0..1 (1 = light full, 0 = off)
    public void HandleIntensityChanged(float intensity)
    {
        // compute target color between original and the darkColor blended by darkenAmount
        Color blendDark = Color.Lerp(originalColor, darkColor, darkenAmount);
        // when intensity==1 -> use blendDark, when intensity==0 -> originalColor
        sr.color = Color.Lerp(originalColor, blendDark, intensity);
    }
}
