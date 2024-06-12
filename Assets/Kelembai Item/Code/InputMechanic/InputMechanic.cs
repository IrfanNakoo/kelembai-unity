using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class InputMechanic : MonoBehaviour
{
    public int maxMessage = 25;
    public GameObject chatPanel, textObject;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        if (chatPanel == null)
        {
            Debug.LogError("chatPanel is not assigned in the Inspector.");
        }
        if (textObject == null)
        {
            Debug.LogError("textObject is not assigned in the Inspector.");
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToChat("You pressed the enter key.");
            Debug.Log("Enter is pressed");
        }

        /*if (!inputBox.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            inputBox.ActivateInputField();
        }*/
    }

    public void SendMessageToChat(string text)
    {
        if (chatPanel == null || textObject == null)
        {
            Debug.LogError("chatPanel or textObject is not assigned.");
            return;
        }

        if (messageList.Count >= maxMessage)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.RemoveAt(0);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);
        if (newText == null)
        {
            Debug.LogError("Failed to instantiate textObject.");
            return;
        }

        newMessage.textObject = newText.GetComponent<Text>();
        if (newMessage.textObject == null)
        {
            Debug.LogWarning("The instantiated textObject does not have a Text component directly on it. Searching children.");
            newMessage.textObject = newText.GetComponentInChildren<Text>();
            if (newMessage.textObject == null)
            {
                Debug.LogError("No Text component found on the instantiated textObject or its children.");
                return;
            }
        }

        newMessage.textObject.text = newMessage.text;
        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
}
