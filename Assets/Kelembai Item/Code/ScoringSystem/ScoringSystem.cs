using UnityEngine;
using TMPro; // For TextMeshPro elements
using VSX.UniversalVehicleCombat; // To access CollisionScanner

public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem instance;

    private int currentScore = 0;
    private int highScore = 0;

    public TMP_Text scoreText;
    public TMP_Text highScoreText;

    public int scorePerEnemy = 100;

    // Ensure a singleton instance
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        highScore = PlayerPrefs.GetInt("HighScore", 0);
        highScoreText.text = "High Score: " + highScore;
    }

    // Adds score based on points and updates the UI
    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: " + currentScore;
        Debug.Log("code nie pulak yang hidup");

        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    // Reset score method
    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "Score: " + currentScore;
    }

    // Getter for high score
    public int GetHighScore()
    {
        return highScore;
    }
}
