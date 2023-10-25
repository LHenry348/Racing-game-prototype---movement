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

    public Transform[] turns = new  Transform[16];
    int dToward;
    private float distance;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void FixedUpdate()
    {
        distance = Vector2.Distance(transform.position, turns[dToward].transform.position);
        Vector2 direction = turns[dToward].transform.position - transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);

        Vector2 engineForceVector = transform.up * 1 * accelerationFactor;

        //rb.AddForce(engineForceVector, ForceMode2D.Force);
    }

}
