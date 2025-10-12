using UnityEngine;

public class BackgroundColour : MonoBehaviour
{
    public bool subscribeAutomatically = true;
    public LegsCollect lightDimmer;
    public Color darkColor = Color.black;
    [Range(0f,1f)] public float darkenAmount = 1f;

    private Camera cam;
    private Color originalColor;

    void Awake()
    {
        cam = GetComponent<Camera>();
        originalColor = cam.backgroundColor;
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
        cam.backgroundColor = Color.Lerp(originalColor, blendDark, intensity);
    }
}
