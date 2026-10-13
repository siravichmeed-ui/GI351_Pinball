
using UnityEngine;

public class Bumper : MonoBehaviour
{
    [Header("Bumper Settings")]
    public float bounceForce = 12f;
    public int scoreValue = 100;
    public float hitCooldown = 0.2f;

    private float lastHitTime = -10f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Ball"))
            return;

        if (Time.time - lastHitTime < hitCooldown)
            return;

        lastHitTime = Time.time;

        Rigidbody2D ballRb = collision.rigidbody;

        if (ballRb != null)
        {
            Vector2 direction =
                (ballRb.position - (Vector2)transform.position).normalized;

            if (direction == Vector2.zero)
                direction = Vector2.up;

            ballRb.AddForce(
                direction * bounceForce,
                ForceMode2D.Impulse
            );
        }

        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }
    }
}
