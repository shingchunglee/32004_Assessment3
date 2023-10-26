using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    private int currentScore = 0;

    public void UpdateScore(int points)
    {
        currentScore += points;
        // Update the UI tag for the score
        scoreText.text = "Score: " + currentScore.ToString();
    }
}
