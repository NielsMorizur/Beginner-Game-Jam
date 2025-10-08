using UnityEngine;
using UnityEngine.Events;

public class GoalCheck : MonoBehaviour
{
    public UnityEvent goalScored;
    private bool scored = false;
    public string ballTag = "Ball";

    private GameObject ball;
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(ballTag) && !scored)
        {
            ball = collision.gameObject;
            Destroy(ball);
            scored = true;
            goalScored?.Invoke();
        }
    }
}
