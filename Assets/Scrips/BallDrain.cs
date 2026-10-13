
using UnityEngine;

public class BallDrain : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Ball"))
            return;

        BallLauncher launcher =
            FindFirstObjectByType<BallLauncher>();

        if (launcher != null)
        {
            launcher.LoseBall();
        }
    }
}
