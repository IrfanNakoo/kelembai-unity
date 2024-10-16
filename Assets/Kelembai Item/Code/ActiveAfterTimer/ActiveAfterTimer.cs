using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ActiveAfterTimer : MonoBehaviour
{
    public GameObject targetObject;            // The object to enable
    public float timerDuration = 5.0f;         // The duration before the object is enabled

    [Header("Events")]
    public UnityEvent onStartTimer;            // Event to trigger the coroutine

    void Start()
    {
        // Optionally, you can call this at the start or trigger it via the Unity event.
        // onStartTimer.Invoke();
    }

    // Call this method to start the coroutine when the event is invoked
    public void StartTimer()
    {
        StartCoroutine(EnableObjectAfterTime(timerDuration));
    }

    // Coroutine to wait for the specified duration before enabling the target object
    private IEnumerator EnableObjectAfterTime(float duration)
    {
        yield return new WaitForSeconds(duration);

        // Enable the target object after the delay
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }
}
