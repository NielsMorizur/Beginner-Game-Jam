using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    private SpriteRenderer sr;
    private float baseAlpha;
    public float flickerAmount = 0.1f;
    public float flickerSpeed = 2f;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        baseAlpha = sr.color.a;
    }

    void Update()
    {
        float offset = Mathf.Sin(Time.time * flickerSpeed) * flickerAmount;
        Color c = sr.color;
        c.a = baseAlpha + offset;
        sr.color = c;
    }
}
