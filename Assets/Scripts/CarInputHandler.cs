using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //components
    CarController carController;

    void Awake()
    {
        carController = GetComponent<CarController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 inputVector = Vector2.zero;
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");
        carController.SetInputVector(inputVector);

        /*if (Input.GetButtonDown("Jump"))
            carController.Jump(1.0f, 0.0f);*/
    }
}
