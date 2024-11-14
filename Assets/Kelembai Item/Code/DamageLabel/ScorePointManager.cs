/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScorePointManager : MonoBehaviour
{
    // Start is called before the first frame update
    // SCRIPT NIE DUDUK DALAM ENEMY SCORE MANAGERs
    // SCRIPT NIE CONTROL SCOREPOINT UNTUK AKTIF/TAK AKTIF
    // SCRIPT NIE AKAN TUNGGU SIGNAL DARI COLLISION SCANNER UNUTK AKTIF

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }



    public static ScorePointManager Instance { get; private set; }
    private int score = 0;

    public 

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score Updated: " + score);
    }

    public int GetScore()
    {
        return score;
    }
}
*/