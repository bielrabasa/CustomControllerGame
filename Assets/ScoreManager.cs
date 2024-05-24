using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public int value = 30;

    int score;
    int timer = 0;
    float auxTimer = 0.0f;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timeText;

    // Start is called before the first frame update
    void Start()
    {
        InitScore();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) SetScore();

        auxTimer += Time.deltaTime;
        timer = (int)auxTimer;
        timeText.text = "Fly time: " + timer.ToString();
    }

    private void OnTriggerEnter(Collider collision)
    {
        if(collision.CompareTag("Building")) SetPosition();


        if (collision.CompareTag("Player")) SetScore();
    }

    void InitScore()
    {
        transform.tag = "Score";

        SetPosition();

        GetComponent<Renderer>().material.color = Color.green;

        score = 0;
    }

    void SetPosition()
    {
        Debug.Log("New Position");
        RandomCity randCity = FindObjectOfType<RandomCity>();

        float posX = Random.Range(0, randCity.size * randCity.buildingSeparation);
        float posZ = Random.Range(0, randCity.size * randCity.buildingSeparation);

        transform.position = new Vector3(posX, 5, posZ);
    }

    void SetScore()
    {
        score += value;

        scoreText.text = "Score: " + score.ToString();

        SetPosition();
    }
}
