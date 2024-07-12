using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveAfterTimer : MonoBehaviour
{
    public GameObject targetObject;
    public float timerDuration = 5.0f;

    void Start()
    {
        StartCoroutine(EnableObjectAfterTime(timerDuration));
    }

    private IEnumerator EnableObjectAfterTime(float duration)
    {
    
        yield return new WaitForSeconds(duration);

       
        if (targetObject != null)
        {
            targetObject.SetActive(true);
        }
    }
}
