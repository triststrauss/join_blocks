using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    Text scoreText;
    int score;

    void Awake()
    {
        scoreText = GetComponentInChildren<Text>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
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