using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoringSystem : MonoBehaviour
{
    public static ScoringSystem instance;  // Singleton for easy access
    public TextMeshProUGUI scoreText;  // Reference to the TextMeshPro UI Text to display the score
    private int score = 0;  // Current score

    void Awake()
    {
        // Ensure there's only one instance of ScoringSystem
        if (instance == null)
        {
            Debug.Log("Scoring system activated.");
            instance = this;
        }
        else
        {
            Debug.LogWarning("Duplicate ScoringSystem detected. Destroying this instance.");
            Destroy(gameObject);
        }
    }

    // Method to increase the score
    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score increased by " + points + ". Current score: " + score);
        UpdateScoreUI();
    }

    // Method to update the score in the UI
    private void UpdateScoreUI()
    {
        scoreText.text = "Score: " + score.ToString();
        Debug.Log("Score UI updated. Displaying: Score: " + score);
    }

    // Optional: Reset the score
    public void ResetScore()
    {
        score = 0;
        Debug.Log("Score reset. Current score: " + score);
        UpdateScoreUI();
    }

    public int GetScore()
    {
        Debug.Log("Current score queried: " + score);
        return score;
    }
}
