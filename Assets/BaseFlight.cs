using System.Collections;
using TMPro;
using Unity.VisualScripting;
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

    Coroutine roll;
    Coroutine exit;

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

        //Roll
        if (Input.GetKeyDown(KeyCode.Space))
        {
            roll = StartCoroutine(Roll(child.localEulerAngles.z > 180));
        }
    }

    IEnumerator Roll(bool right)
    {
        playing = false;
        float totalRoll = 360f;
        float totalSide = 10f; //Real roll = totalSide / 2 +-

        while(totalRoll > 0f)
        {
            float toRotate = 700f * Time.deltaTime;
            totalRoll -= toRotate;
            transform.Translate((right ? Vector3.right : Vector3.left) * totalSide * Time.deltaTime);
            child.Rotate((right ? Vector3.back : Vector3.forward) * toRotate);
            yield return null;
        }

        playing = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Die();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Map"))
        {
            exit = StartCoroutine(MapExit());
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (exit != null && other.CompareTag("Map"))
        {
            StopCoroutine(exit);
            text.text = "";
        }
    }

    IEnumerator MapExit(){
        text.text = "Return to map!";
        yield return new WaitForSeconds(2);

        for (int i = 5; i > 0; i--)
        {
            text.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        text.text = "";
        Die();
    }

    void Die()
    {
        playing = false;
        GetComponent<ParticleSystem>().Play();

        Destroy(rb);
        if (roll != null) StopCoroutine(roll);
        StartCoroutine(Restart());
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
