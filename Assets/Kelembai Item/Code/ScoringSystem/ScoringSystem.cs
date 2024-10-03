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
    public CollisionScanner collisionScanner;

    void Awake()
    {
        // Ensure a singleton instance
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

    void Start()
    {
        // Subscribe to collision event
        if (collisionScanner != null)
        {
            collisionScanner.onHitDetected.AddListener(OnEnemyHit);
            collisionScanner.CallingScoringSystem = this;
        }
    }

    void OnDestroy()
    {
        // Unsubscribe from the event
        if (collisionScanner != null)
        {
            collisionScanner.onHitDetected.RemoveListener(OnEnemyHit);
        }
    }

    void OnEnemyHit(RaycastHit hit)
    {
        AddScore(scorePerEnemy);
    }

    public void AddScore(int points)
    {
        currentScore += points;
        scoreText.text = "Score: " + currentScore;

        if (currentScore > highScore)
        {
            highScore = currentScore;
            highScoreText.text = "High Score: " + highScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
    }

    public void ResetScore()
    {
        currentScore = 0;
        scoreText.text = "Score: " + currentScore;
    }

    public int GetHighScore()
    {
        return highScore;
    }
}
