using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public RawImage compassImage; // Assign a RawImage with a repeating compass texture
    public Transform player;     // Assign the player's transform

    private void Update()
    {
        // Normalize the player's Y rotation
        float uvOffset = player.eulerAngles.y / 360f;

        // Ensure UV offset stays within [0, 1]
        uvOffset = uvOffset % 1f;

        // Update the compass texture's UV Rect
        compassImage.uvRect = new Rect(uvOffset, 0f, 1f, 1f);

    }
}

//something wrong
//maybe dengan compass bar