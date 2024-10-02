using UnityEngine;
using TMPro; // For TextMeshPro elements, if needed
using UnityEngine.UI;  // For displaying the score on the UI

public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem instance;

    // The current score and high score
    private int currentScore = 0;
    private int highScore = 0;

    // Reference to the UI text element that displays the current score
    public TMP_Text scoreText;

    // Reference to the UI text element that displays the high score
    public TMP_Text highScoreText;

    // Score to add when an enemy is destroyed
    public int scorePerEnemy = 100;

    void Start()
    {
        // Ensure the highScoreText is updated on game start
        highScore = GetHighScore();  // Use the class-level highScore
        highScoreText.text = "High Score: " + highScore;
    }

    void Awake()
    {
        // Singleton pattern to make sure only one ScoreManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); // Persist between scenes if needed
        }
        else
        {
            Destroy(gameObject);
        }

        // Load the high score from player preferences
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    // Function to add points to the score
    public void AddScore(int points)
    {
        currentScore += points;
        UpdateScoreUI();

        // Check if the new score is higher than the high score
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save(); // Ensure the high score is saved
            highScoreText.text = "High Score: " + highScore;
        }
    }

    // Function to update the score UI
    void UpdateScoreUI()
    {
        scoreText.text = "Score: " + currentScore;
    }

    // Optional function to reset score when a new game starts
    public void ResetScore()
    {
        currentScore = 0;
        UpdateScoreUI();
    }

    // Optional function to get the high score
    public int GetHighScore()
    {
        return highScore;
    }
}
