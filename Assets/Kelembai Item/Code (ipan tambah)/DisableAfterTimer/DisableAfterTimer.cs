using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAfterTimer : MonoBehaviour
{
    public GameObject targetObject;
    public float timerDuration = 5.0f;

    
    void Start()
    {
        
        StartCoroutine(DisableObjectAfterTime(timerDuration));
    }

    
    private IEnumerator DisableObjectAfterTime(float duration)
    {
        
        yield return new WaitForSeconds(duration);

        
        if (targetObject != null)
        {
            targetObject.SetActive(false);
        }
    }
}
