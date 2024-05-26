using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Score : MonoBehaviour
{
    int score;
    public TextMeshProUGUI scoreText;
    public int coinNumber;
    public GameObject coin;

    public TextMeshProUGUI timeText;
    int timer = 0;
    float auxTimer = 0.0f;

    void Start()
    {
        score = 0;
        scoreText.text = "Score: " + score.ToString();

        for (int i = 0; i < coinNumber; i++)
        {
            Instantiate(coin, transform);
        }
    }

    private void Update()
    {
        auxTimer += Time.deltaTime;
        timer = (int)auxTimer;
        timeText.text = "Fly time: " + timer.ToString();
    }

    public void AddScore(int n)
    {
        score += n;
        scoreText.text = "Score: " + score.ToString();
    }
}
