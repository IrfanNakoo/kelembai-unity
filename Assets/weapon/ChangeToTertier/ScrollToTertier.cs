using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollToTertier : MonoBehaviour
{
    GeneralInputAsset defaultControl;
    public float mouseScrollY;

    private void Awake()
    {
        defaultControl = new GeneralInputAsset();

        defaultControl.WeaponControls.ScrollToTertier.performed += ctx => mouseScrollY = ctx.ReadValue<float>();
    }

    private void Update()
    {
        if (mouseScrollY > 0)
        {
            Debug.Log("scrolled Up");
            mouseScrollY = 0; // Reset to prevent multiple logs
        }
        else if (mouseScrollY < 0)
        {
            Debug.Log("scrolled Down");
            mouseScrollY = 0; // Reset to prevent multiple logs
        }
    }

    void OnEnable()
    {
        defaultControl.Enable();
    }

    void OnDisable()
    {
        defaultControl.Disable();
    }
}