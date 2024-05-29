using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseFlight : MonoBehaviour
{
    Rigidbody rb;
    Transform child;

    ArduinoConnection arduino;
    float xRot = 0;
    public float planeSpeed;
    public float rotSpeed;

    public TMP_Text text;
    bool playing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        child = transform.GetChild(0);
        arduino = FindObjectOfType<ArduinoConnection>();
        playing = false;

        StartCoroutine(Countdown());
    }


    void Update()
    {
        if (!playing) return;

        Vector3 finalRot = arduino.acceleration;
        xRot -= finalRot.x * rotSpeed * Time.deltaTime;

        child.localEulerAngles = new Vector3(0, 0, finalRot.x * 90);

        finalRot.x = Mathf.Sin(xRot * Mathf.Deg2Rad);
        finalRot.z = Mathf.Cos(xRot * Mathf.Deg2Rad);
        
        transform.forward = Vector3.Lerp(finalRot.normalized, transform.forward, 0.80f);
        rb.velocity = transform.forward * planeSpeed;
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
        playing = true;
        yield return new WaitForSeconds(1);
        text.text = "";
    }

    private void OnCollisionEnter(Collision collision)
    {
        playing = false;
        Destroy(rb);
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
