using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoringSystem : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    /*
    public int baseScorePerKill = 10;
    public int headshotMultiplier = 2;
    public int streakBonus = 50;
    public int objectiveBonus = 100;
    public float accuracyMultiplier = 0.2f; // 20% of the accuracy percentage
    public float difficultyMultiplier = 1.5f; // Assuming Hard difficulty

    private int currentScore = 0;
    private int killStreak = 0;
    private int totalKills = 0;
    private int headshotCount = 0;
    private int totalShotsFired = 0;
    private int totalShotsHit = 0;

    public void AddKill(bool isHeadshot)
    {
        int points = baseScorePerKill;

        if (isHeadshot)
        {
            points *= headshotMultiplier;
            headshotCount++;
        }

        totalKills++;
        killStreak++;

        if (killStreak % 5 == 0) // Every 5 kills, add streak bonus
        {
            points += streakBonus;
        }

        currentScore += points;
        Debug.Log($"Score: {currentScore}");
    }

    public void AddShotFired()
    {
        totalShotsFired++;
    }

    public void AddShotHit()
    {
        totalShotsHit++;
    }

    public void CompleteObjective()
    {
        currentScore += objectiveBonus;
    }

    public void CalculateFinalScore()
    {
        float accuracy = (float)totalShotsHit / totalShotsFired;
        int accuracyBonus = Mathf.RoundToInt(accuracy * accuracyMultiplier * 100); // 20% of accuracy as bonus
        currentScore += accuracyBonus;

        currentScore = Mathf.RoundToInt(currentScore * difficultyMultiplier);

        Debug.Log($"Final Score: {currentScore}");
    }
    */



    
    public GameObject objectA; // Assign objectA in the Unity Editor
    public int score = 0; // Initial score

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the enemy collided with objectA
        if (collision.gameObject == objectA)
        {
            // Increase the score by 10
            score += 10;
            Debug.Log("Enemy hit objectA. Score: " + score);

            // Destroy the enemy GameObject
            Destroy(gameObject);
        }
    }

}
