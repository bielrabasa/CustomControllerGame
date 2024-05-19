using Microsoft.Win32;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlight : MonoBehaviour
{
    public float startVelocity = 10;

    //public float steerForce = 20;
    //public float turnForce = 10;

    Rigidbody rb;
    //Vector3 rot;

    ArduinoConnection arduino;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arduino = FindObjectOfType<ArduinoConnection>();
        //rot = transform.eulerAngles;

        rb.velocity = Vector3.forward * startVelocity;
    }


    void Update()
    {
        /*rot.x += steerForce * Input.GetAxis("Vertical") * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, -30, 45);

        float axisH = Input.GetAxis("Horizontal");
        rot.y += steerForce * axisH * Time.deltaTime;

        //if(axisH < 0.1f) rot.z //TODO: Tornar a 0 gradualment
        rot.z += -turnForce * axisH * Time.deltaTime;
        rot.z = Mathf.Clamp(rot.z, -10, 10);

        transform.rotation = Quaternion.Euler(rot);*/

        Vector3 acc = arduino.acceleration;
        acc.x *= -1;
        transform.forward = Vector3.Lerp(acc, transform.forward, 0.99f);

        float dot = Vector3.Dot(transform.forward, Vector3.up);
        rb.velocity = transform.forward * (rb.velocity.magnitude - dot * Time.deltaTime);
    }
}
