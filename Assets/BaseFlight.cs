using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseFlight : MonoBehaviour
{
    float percentage;

    Rigidbody rb;
    Vector3 rot;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rot = transform.eulerAngles;
    }


    void Update()
    {
        rot.x += 20 * Input.GetAxis("Vertical") * Time.deltaTime;
        rot.x = Mathf.Clamp(rot.x, -30, 45);

        rot.y += 20 * Input.GetAxis("Horizontal") * Time.deltaTime;

        rot.z = -5 * Input.GetAxis("Horizontal");
        rot.z = Mathf.Clamp(rot.z, -5, 5);

        transform.rotation = Quaternion.Euler(rot);

        //percentage = rot.x / 45f;

        //float mod_drag = (percentage * -2) + 6;
        //float mod_speed = percentage * (13.8f - 12.5f) + 12.5f; //TODO: Fixed, change

        //rb.drag = mod_drag;
        Vector3 localV = transform.InverseTransformDirection(rb.velocity);

        //localV.z = mod_speed; // - (Vector3.Dot(transform.up, Vector3.up) * Time.deltaTime);
        localV.z += (Vector3.Dot(transform.up, Vector3.up) * 100f * Time.deltaTime);

        Vector3 vel = transform.TransformDirection(localV);

        rb.velocity = vel;
    }
}
