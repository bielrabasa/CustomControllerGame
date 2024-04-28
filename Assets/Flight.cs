using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flight : MonoBehaviour
{
    Rigidbody rb;
    public float rotationSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.forward * 10;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotDif = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow)) rotDif.x -= rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.DownArrow)) rotDif.x += rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow)) rotDif.y -= rotationSpeed * Time.deltaTime;

        if (Input.GetKey(KeyCode.RightArrow)) rotDif.y += rotationSpeed * Time.deltaTime;

        if(rotDif != Vector3.zero)
        {
            transform.Rotate(rotDif, Space.World);
            rb.velocity = transform.forward * rb.velocity.magnitude;
        }
    }
}
