using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    Rigidbody2D rb;
    public float driftFactor = 0.95f;
    public float accelerationFactor = 30.0f;
    public float turnFactor = 3.5f;
    public float maxSpeed = 20;
    public float turnRange;

    //public Transform[] turns = new  Transform[16];
    Transform[] turns;
    GameObject controller;
    int dToward;
    float distance;

    float accelerationInput = 0;
    float velocityVsUp = 0;

    private void Awake()
    {
        controller = GameObject.Find("EnemyController");
        turns = new Transform[controller.transform.childCount];
        for(int i = 0; i < turns.Length; i++)
        {
            turns[i] = controller.transform.GetChild(i);
        }
        rb = GetComponent<Rigidbody2D>();
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
    }

    void KillOrthogonalVelocity()
    {
        Vector2 forwardVelocity = transform.up * Vector2.Dot(rb.velocity, transform.up);
        Vector2 rightVelocity = transform.right * Vector2.Dot(rb.velocity, transform.right);
        rb.velocity = forwardVelocity + rightVelocity * driftFactor;
    }
}
