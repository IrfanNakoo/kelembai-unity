using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputMechanic : MonoBehaviour
{
    public InputField inputBoxLegacy;
    public static bool isInputActive = false; // start mati

    public int maxMessage = 25;
    public GameObject chatPanel, textObject;
    public InputField inputBox;

    public Color playerMessage, info;

    [SerializeField]
    private List<Message> messageList = new List<Message>();

    void Start()
    {
        if (inputBoxLegacy != null)
        {
            inputBoxLegacy.gameObject.SetActive(false); // start mati
            isInputActive = false;
        }

        if (chatPanel == null)
        {
            chatPanel = GameObject.FindWithTag("ChatPanel");
            if (chatPanel == null)
            {
                Debug.LogError("chatPanel is not assigned and could not be found with the tag 'ChatPanel'.");
            }
        }

        if (textObject == null)
        {
            Debug.LogError("textObject is not assigned.");
        }

        if (inputBox == null)
        {
            inputBox = FindObjectOfType<InputField>();
            if (inputBox == null)
            {
                Debug.LogError("inputBox is not assigned and could not be found dynamically.");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isInputActive) // kalau ad perkataan
            {
                if (!string.IsNullOrEmpty(inputBox.text)) // klau ad perkataan dia hantar ke SendMessageToChat
                {
                    SendMessageToChat(inputBox.text, Message.MessageType.playerMessage);
                    inputBox.text = "";
                }
                inputBox.DeactivateInputField();
                isInputActive = false;
            }
            else
            {
                ToggleInputField(); // kalau tak ada perkataan
            }
        }
    }

    public void ToggleInputField() // klau tak ada perkataan (dia fikir input box mati)
    {
        if (inputBoxLegacy != null) //check wujud ke tk
        {
            isInputActive = !isInputActive;
            inputBoxLegacy.gameObject.SetActive(isInputActive);

            if (isInputActive) // kalau mati hidupkan
            {
                inputBoxLegacy.Select();
                inputBoxLegacy.ActivateInputField();
            }
            else // kalau hidup matikan
            {
                inputBoxLegacy.DeactivateInputField();
                inputBoxLegacy.gameObject.SetActive(false);
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
        Text newTextComponent = newText.GetComponent<Text>() ?? newText.GetComponentInChildren<Text>();
        if (newTextComponent == null)
        {
            Debug.LogError("No Text component found on the instantiated textObject or its children.");
            return;
        }

        newMessage.textObject = newTextComponent;
        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType)
    {
        return messageType == Message.MessageType.playerMessage ? playerMessage : info;
    }
}

[System.Serializable]
public class Message
{
    public string text;
    public Text textObject;
    public enum MessageType
    {
        playerMessage,
        info
    }
}
