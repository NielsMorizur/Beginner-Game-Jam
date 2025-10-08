using UnityEngine;
using System.Collections;

public class OpenLeverDoor : MonoBehaviour
{
    public float endAngle = 90f;
    public float duration = 3f;

    public void OpenDoor()
    {
        StartCoroutine(RotateOverTime());
    }
    
    private IEnumerator RotateOverTime()
    {
        float startAngle = transform.eulerAngles.z;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float newZ = Mathf.LerpAngle(startAngle, endAngle, t);
            transform.rotation = Quaternion.Euler(0, 0, newZ);
            yield return null;
        }

        transform.rotation = Quaternion.Euler(0, 0, endAngle);
    }
}
