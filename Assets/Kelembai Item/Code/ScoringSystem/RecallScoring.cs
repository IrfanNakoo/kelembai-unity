using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecallScoring : MonoBehaviour
{
    public TMP_Text highScoreText;  // UI element to display the high score
    public TMP_Text accuracyText;   // UI element to display the accuracy

    private int highScore;          // Variable to store high score

    void Start()
    {
        // Retrieve the saved high score when the game starts
        highScore = PlayerPrefs.GetInt("HighScore", 0);  // Default to 0 if not found
        highScoreText.text = "" + highScore;

        // Retrieve accuracy from PlayerPrefs
        float storedAccuracy = PlayerPrefs.GetFloat("PlayerAccuracy", 0f); // Default to 0 if not found
        accuracyText.text = "" + storedAccuracy.ToString("F2") + "%";
    }
}
