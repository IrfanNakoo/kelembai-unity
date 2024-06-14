using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; // Include this for TextMeshPro

public class InputMechanic : MonoBehaviour
{
    public int maxMessage = 25;
    public GameObject chatPanel, textObject;
    public InputField inputBox;

    public Color playerMessage, info;

    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        if (chatPanel == null)
        {
            chatPanel = GameObject.FindWithTag("ChatPanel"); // Example: Find by tag
            if (chatPanel == null)
            {
                Debug.LogError("chatPanel is not assigned in the Inspector and could not be found dynamically.");
            }
        }
        if (textObject == null)
        {
            Debug.LogError("textObject is not assigned in the Inspector.");
        }
        if (inputBox == null)
        {
            inputBox = FindObjectOfType<InputField>();
            if (inputBox == null)
            {
                Debug.LogError("inputBox is not assigned in the Inspector and could not be found dynamically.");
            }
        }
    }

    void Update()
    {
        if (inputBox != null && inputBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat(inputBox.text, Message.MessageType.playerMessage);
                inputBox.text = "";
            }
        }
        else
        {
            if (inputBox != null && !inputBox.isFocused && Input.GetKeyDown(KeyCode.Return))
            {
                inputBox.ActivateInputField();
            }
        }

        if (inputBox != null && !inputBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChat("You pressed the enter key.", Message.MessageType.info);
                Debug.Log("Enter is pressed");
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
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

        Message newMessage = new Message { text = text };

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
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = info;

        switch (messageType)
        {
            case Message.MessageType.playerMessage:
                color = playerMessage;
                break;
        }

        return color;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        info
    }
}
