using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovementScript : MonoBehaviour
{
    public float MovementSpeed = 10f;
    public GameObject car;
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        // get input
        float x = Input.GetAxis("Horizontal") * MovementSpeed * Time.deltaTime;
        float y = Input.GetAxis("Vertical") * MovementSpeed * Time.deltaTime;
        transform.Translate(x, y, 0);
        bool k = Input.GetKeyDown(KeyCode.K);

        if (x != 0 || y != 0)
        {
            float angle = Mathf.Atan2(y, x) * Mathf.Rad2Deg;
            //car.transform.rotation = Quaternion.Euler(0, 0, angle);
            Player.transform.rotation = Quaternion.Euler(0, 0, angle);
        }
        if (Input.GetKeyDown(KeyCode.K))
        {
            MovementSpeed = 7f;
        }
        else if (Input.GetKeyUp(KeyCode.K))
        {
            MovementSpeed = 10f;
        }
    }
}