using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseFlight : MonoBehaviour
{
    public float startVelocity = 10;

    //public float steerForce = 20;
    //public float turnForce = 10;

    Rigidbody rb;
    //Vector3 rot;

    ArduinoConnection arduino;

    TMP_Text text;
    bool playing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        arduino = FindObjectOfType<ArduinoConnection>();
        playing = false;

        text = FindObjectOfType<TMP_Text>();
        StartCoroutine(Countdown());
    }


    void Update()
    {
        if (!playing) return;
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

    IEnumerator Countdown()
    {
        text.text = "";
        yield return new WaitForSeconds(1);
        text.text = "Ready?";
        yield return new WaitForSeconds(1);
        text.text = "Set";
        yield return new WaitForSeconds(1);
        text.text = "GO";
        rb.velocity = transform.forward * startVelocity;
        playing = true;
        yield return new WaitForSeconds(1);
        text.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        text.text = "Crash";
        yield return new WaitForSeconds(1);
        text.text = "Cras";
        yield return new WaitForSeconds(0.2f);
        text.text = "Cra";
        yield return new WaitForSeconds(0.2f);
        text.text = "Cr";
        yield return new WaitForSeconds(0.2f);
        text.text = "C";
        yield return new WaitForSeconds(0.2f);
        text.text = "";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
