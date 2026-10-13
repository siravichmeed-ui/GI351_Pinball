
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BallLauncher : MonoBehaviour
{
    [Header("Ball")]
    public Rigidbody2D ball;
    public Transform launchPosition;

    [Header("Ball Count")]
    public int startingBalls = 3;
    public int ballsRemaining;

    [Header("Charge Settings")]
    public float minForce = 12f;
    public float maxForce = 22f;
    public float chargeTime = 2f;

    [Header("Charge UI")]
    public Slider chargeSlider;

    [Header("Lives UI")]
    public LivesUI livesUI;

    [Header("Ball Trail")]
    public TrailRenderer ballTrail;

    private bool launched;
    private bool charging;
    private bool exitedGate;
    private bool gameOver;

    private float chargeTimer;
    private Coroutine trailResetCoroutine;

    private void Start()
    {
        if (ball == null || launchPosition == null)
        {
            Debug.LogError(
                "Assign Ball and Launch Position!", this);
            enabled = false;
            return;
        }

        if (ballTrail == null)
        {
            ballTrail = ball.GetComponent<TrailRenderer>();
        }

        if (ballTrail != null)
        {
            ballTrail.enabled = true;
            ballTrail.emitting = true;
            ballTrail.Clear();
        }
        else
        {
            Debug.LogError(
                "Trail Renderer not found on Ball!", this);
        }

        ballsRemaining = startingBalls;

        if (chargeSlider != null)
        {
            chargeSlider.minValue = 0f;
            chargeSlider.maxValue = 1f;
            chargeSlider.value = 0f;
        }

        if (livesUI != null)
        {
            livesUI.UpdateLives(ballsRemaining);
        }

        ResetBall();
    }

    private void Update()
    {
        if (gameOver || launched)
            return;

        if (!charging && Input.GetKeyDown(KeyCode.Space))
        {
            charging = true;
            chargeTimer = 0f;
        }

        if (charging && Input.GetKey(KeyCode.Space))
        {
            chargeTimer += Time.deltaTime;
            chargeTimer = Mathf.Min(
                chargeTimer, chargeTime);

            UpdateChargeUI();
        }

        if (charging && Input.GetKeyUp(KeyCode.Space))
        {
            charging = false;
            launched = true;

            float chargePercent = chargeTime > 0f
                ? chargeTimer / chargeTime
                : 1f;

            float force = Mathf.Lerp(
                minForce,
                maxForce,
                chargePercent);

            ball.bodyType = RigidbodyType2D.Dynamic;
            ball.linearVelocity = Vector2.zero;
            ball.angularVelocity = 0f;

            // เริ่มหางใหม่เมื่อยิง
            if (ballTrail != null)
            {
                ballTrail.Clear();
                ballTrail.enabled = true;
                ballTrail.emitting = true;
            }

            ball.AddForce(
                Vector2.up * force,
                ForceMode2D.Impulse);

            if (chargeSlider != null)
            {
                chargeSlider.value = 0f;
            }
        }
    }

    private void UpdateChargeUI()
    {
        if (chargeSlider != null)
        {
            chargeSlider.value = chargeTime > 0f
                ? chargeTimer / chargeTime
                : 1f;
        }
    }

    public void MarkBallExitedGate()
    {
        exitedGate = true;
    }

    public void LoseBall()
    {
        if (!launched || gameOver)
            return;

        // ยังไม่ผ่าน Gate: กลับไปยิงใหม่ ไม่เสียลูก
        if (!exitedGate)
        {
            ResetBall();
            return;
        }

        ballsRemaining--;

        if (livesUI != null)
        {
            livesUI.UpdateLives(ballsRemaining);
        }

        Debug.Log("Balls Remaining: " + ballsRemaining);

        if (ballsRemaining <= 0)
        {
            ballsRemaining = 0;
            gameOver = true;

            if (ballTrail != null)
            {
                ballTrail.emitting = false;
                ballTrail.Clear();
            }

            ball.gameObject.SetActive(false);

            Debug.Log("GAME OVER");
            return;
        }

        ResetBall();
    }

    private void ResetBall()
    {
        if (trailResetCoroutine != null)
        {
            StopCoroutine(trailResetCoroutine);
            trailResetCoroutine = null;
        }

        // ปิดการสร้างหางก่อนย้ายลูกบอล
        if (ballTrail != null)
        {
            ballTrail.emitting = false;
            ballTrail.Clear();
        }

        ball.linearVelocity = Vector2.zero;
        ball.angularVelocity = 0f;
        ball.bodyType = RigidbodyType2D.Kinematic;

        ball.position = launchPosition.position;
        ball.transform.position = launchPosition.position;

        launched = false;
        charging = false;
        exitedGate = false;
        chargeTimer = 0f;

        if (chargeSlider != null)
        {
            chargeSlider.value = 0f;
        }

        ball.gameObject.SetActive(true);

        trailResetCoroutine = StartCoroutine(
            ResetTrailNextFrame());
    }

    private IEnumerator ResetTrailNextFrame()
    {
        yield return null;

        if (ballTrail != null && ball != null && !gameOver)
        {
            ballTrail.Clear();
            ballTrail.enabled = true;
            ballTrail.emitting = true;
        }

        trailResetCoroutine = null;
    }
}
