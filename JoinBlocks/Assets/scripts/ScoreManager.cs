using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    Text scoreText;
    int score;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponentInChildren<Text>();
    }

    public void Update()
    {

    }

    internal void incrementScore(int incrementScore)
    {
        score += incrementScore;
        scoreText.text = score + "";
    }
}