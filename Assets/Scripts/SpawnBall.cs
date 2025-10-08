using UnityEngine;

public class SpawnBall : MonoBehaviour
{
    public GameObject ball;
    private GameObject createdBall = null; 
    private bool hasSpawned = false;

    public void Spawn()
    {
        if (ball == null) return;
        if (GameManager.Instance.goalScored) return;

        if (hasSpawned)
        {
            Destroy(createdBall);
        }

        createdBall = Instantiate(ball, transform.position, Quaternion.identity);

        hasSpawned = true;
    }
}
