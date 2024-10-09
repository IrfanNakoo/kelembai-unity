using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RecallScoring : MonoBehaviour
{
   public TMP_Text highScoreText;

   void Start()
   {
       // Retrieve the saved high score
       int highScore = PlayerPrefs.GetInt("HighScoreTutorial", 0);  // Default to 0 if no score is found

       // Update the UI to show the high score
       highScoreText.text = "Score: " + highScore;
   }
}