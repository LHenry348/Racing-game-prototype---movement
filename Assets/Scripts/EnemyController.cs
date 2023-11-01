using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    Collider2D enemyCollider;
    SurfaceDetection surfaceDetection;
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;
    public float turnRange;

    public SpriteRenderer carSpriteRenderer;
    public SpriteRenderer carShadowRenderer;

    //public Transform[] turns = new  Transform[16];
    Transform[] turns;
    GameObject controller;
    int dToward;
    float distance;

    float accelerationInput = 0;
    float velocityVsUp = 0;
    bool isJumping = false;

    public AnimationCurve jumpCurve;
    public ParticleSystem landingParticles;

    private void Awake()
    {
        controller = GameObject.Find("EnemyController");
        turns = new Transform[controller.transform.childCount];
        for(int i = 0; i < turns.Length; i++)
        {
            turns[i] = controller.transform.GetChild(i);
        }
        rb = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponentInChildren<Collider2D>();
        surfaceDetection = GetComponent<SurfaceDetection>();
    }
    void FixedUpdate()
    {
        if (GameManager.instance.GetGameState() == GameStates.countDown)
        {
            return;
        }
        Turn();
        Vector2 direction = turns[dToward].transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
        ApplyEngineForce();
        KillOrthogonalVelocity();

    }

    void Turn()
    {
        distance = Vector2.Distance(transform.position, turns[dToward].transform.position);
        if(distance > turnRange)
        {
            accelerationInput = 1;
        }
        else
        {
            accelerationInput = 0;
        }
        if(distance < turnRange)
        {
            dToward++;
        }
        if(dToward == turns.Length)
        {
            dToward = 0;
        }
    }
    void ApplyEngineForce()
    {
        if (isJumping && accelerationInput < 0)
        {
            accelerationInput = 0;
        }
        //calculate how much forward we are going in terms of the direction of our velocity
        velocityVsUp = Vector2.Dot(transform.up, rb.velocity);

        //limit so we cannot go faster than the max speed going forwards
        if (velocityVsUp > maxSpeed && accelerationInput > 0)
            return;

        //limit so we cannot go faster than the 50% of max speed in reverse
        if (velocityVsUp < -maxSpeed * 0.5f && accelerationInput < 0)
            return;

        //limit so we cannot go faster in any direction while accelerating
        if (rb.velocity.sqrMagnitude > maxSpeed * maxSpeed && accelerationInput > 0)
            return;

        //apply drag if there is no accelerationInput so the car stops when the player lets go of the accelerator
        if (accelerationInput == 0)
            rb.drag = Mathf.Lerp(rb.drag, 3.0f, Time.fixedDeltaTime * 3);
        else rb.drag = 0;

        //create a force for the engine
        Vector2 engineForceVector = transform.up * accelerationInput * accelerationFactor;

        //apply force and pushes the car forward
        rb.AddForce(engineForceVector, ForceMode2D.Force);

        switch (GetSurface())
        {
            case Offroad.SurfaceTypes.Grass:
                rb.drag = Mathf.Lerp(rb.drag, 9.0f, Time.fixedDeltaTime * 3);
                break;
        }
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        float currentDriftFactor = driftFactor;
        rb.velocity = forwardVelocity + rightVelocity * currentDriftFactor;

        switch (GetSurface())
        {
            case Offroad.SurfaceTypes.Grass:
                currentDriftFactor *= 1.05f;
                break;
        }
    }

    public Offroad.SurfaceTypes GetSurface()
    {
        return surfaceDetection.GetCurrentSurface();
    }

    public void Jump(float jumpHeightScale, float jumpPushScale)
    {
        if (!isJumping)
            StartCoroutine(JumpCo(jumpHeightScale, jumpPushScale));
    }

    private IEnumerator JumpCo(float jumpHeightScale, float jumpPushScale)
    {
        isJumping = true;

        float jumpStartTime = Time.time;
        float jumpDuration = rb.velocity.magnitude * 0.05f;

        jumpHeightScale = jumpHeightScale * rb.velocity.magnitude * 0.05f;
        jumpHeightScale = Mathf.Clamp(jumpHeightScale, 0.0f, 1.0f);

        enemyCollider.enabled = false;

        carSpriteRenderer.sortingLayerName = "Flying";
        carShadowRenderer.sortingLayerName = "Flying";

        rb.AddForce(rb.velocity.normalized * jumpPushScale * 10, ForceMode2D.Impulse);

        while (isJumping)
        {
            float jumpCompletedPercentage = (Time.time - jumpStartTime) / jumpDuration;
            jumpCompletedPercentage = Mathf.Clamp01(jumpCompletedPercentage);

            carSpriteRenderer.transform.localScale = Vector3.one + Vector3.one * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale * 0.75f;

            carShadowRenderer.transform.localPosition = new Vector3(1, -1, 0.0f) * jumpCurve.Evaluate(jumpCompletedPercentage) * jumpHeightScale;

            if (jumpCompletedPercentage == 1.0f)
                break;

            yield return null;
        }

        if (Physics2D.OverlapCircle(transform.position, 1.5f))
        {
            isJumping = false;

            Jump(0.2f, 0.6f);
        }

        else
        {
            carSpriteRenderer.transform.localScale = Vector3.one;

            carShadowRenderer.transform.localPosition = Vector3.zero;
            carShadowRenderer.transform.localScale = carSpriteRenderer.transform.localScale;

            enemyCollider.enabled = true;

            carSpriteRenderer.sortingLayerName = "Default";
            carShadowRenderer.sortingLayerName = "Default";

            if (jumpHeightScale > 0.2f)
            {
                landingParticles.Play();
            }


            isJumping = false;
        }
    }
    void OnTriggerEnter2D(Collider2D collider2d)
    {
        if (collider2d.CompareTag("Jump"))
        {
            JumpData jumpData = collider2d.GetComponent<JumpData>();
            Jump(jumpData.jumpHeightScale, jumpData.jumpPushScale);
        }
    }
}
