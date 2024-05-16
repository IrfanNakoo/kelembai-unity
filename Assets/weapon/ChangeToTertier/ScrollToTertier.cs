using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScrollToTertier : MonoBehaviour
{
    public GameObject SecondaryWeapon;
    public GameObject TertierWeapon;

    private GeneralInputAsset defaultControl;
    private float mouseScrollY;

    private void Awake()
    {
        TertierWeapon.SetActive(false); // Ensure the tertiary weapon is inactive initially

        defaultControl = new GeneralInputAsset();

        defaultControl.WeaponControls.ScrollToTertier.performed += ctx => mouseScrollY = ctx.ReadValue<float>();
    }

    private void Update()
    {
        if (mouseScrollY < 0)
        {
            if (SecondaryWeapon.activeSelf)
            {
                SecondaryWeapon.SetActive(false);
                TertierWeapon.SetActive(true);
            }

            Debug.Log("scrolled Up");
            mouseScrollY = 0; // Reset to prevent multiple logs
        }
        else if (mouseScrollY > 0)
        {
            if (!SecondaryWeapon.activeSelf)
            {
                SecondaryWeapon.SetActive(true);
                TertierWeapon.SetActive(false);
            }

            Debug.Log("scrolled Down");
            mouseScrollY = 0; // Reset to prevent multiple logs
        }
    }

    private void OnEnable()
    {
        defaultControl.Enable();
    }

    private void OnDisable()
    {
        defaultControl.Disable();
    }
}