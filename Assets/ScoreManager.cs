using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    ArduinoConnection arduino;

    int value;

    // Start is called before the first frame update
    void Start()
    {
        InitScore();
        arduino = FindObjectOfType<ArduinoConnection>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, Time.deltaTime * 180, Space.World);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Building")) SetPosition();


        if (collision.CompareTag("Player")) SetScore();
    }

    void InitScore()
    {
        SetPosition();

        GetComponent<Renderer>().material.color = Color.yellow;
    }

    void SetPosition()
    {
        RandomCity randCity = FindObjectOfType<RandomCity>();

        float posX = Random.Range(0, randCity.size * randCity.buildingSeparation);
        float posZ = Random.Range(0, randCity.size * randCity.buildingSeparation);

        float posY = Random.Range(1, randCity.height);
        value = (int)((50 - posY) / 10);

        transform.position = new Vector3(posX, posY, posZ);
    }

    void SetScore()
    {
        transform.parent.GetComponent<Score>().AddScore(value);
        arduino.checkVibration("W");
        SetPosition();
    }
}
