using UnityEngine;
using System.Collections;

public class OpenDoor2 : MonoBehaviour
{
    public float endY = -0.92f;
    public float duration = 3f;

    public void OpenDoor()
    {
        StartCoroutine(RotateOverTime());
    }
    
    private IEnumerator RotateOverTime()
    {
        float startY = transform.position.y;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newY = Mathf.Lerp(startY, endY, t);
            transform.position = new Vector3(0, newY, 0);
            yield return null;
        }

        transform.position = new Vector3(0, endY, 0);
        enabled = false;
    }
}
