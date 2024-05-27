using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    float remainingTime = 5f; // Initialize the timer with 5 seconds

    public string sceneName;

    public GameObject main_camera; // Assuming main_camera is a reference to the camera GameObject

    // Update is called once per frame
    void Update()
    {
        if (main_camera != null)
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60);
            int seconds = Mathf.FloorToInt(remainingTime % 60);
            timerText.text = string.Format("{0:00} : {1:00}", minutes, seconds);

            if (remainingTime <= 0)
            {
                remainingTime = 0;
                TimerEnded();
            }
        }
        else
        {
            // Handle the case where main_camera is not assigned or null
            Debug.LogWarning("Main camera not assigned.");
        }
    }

    void TimerEnded()
    {
        // Actions to take when the timer ends
        Debug.Log("Timer has ended!");
        // You can add any additional actions here, such as stopping gameplay, showing a message, etc.
        SceneManager.LoadScene(sceneName);
    }
}