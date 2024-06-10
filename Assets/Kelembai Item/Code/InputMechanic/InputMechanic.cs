using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Make sure to include this for InputField
using UnityEngine.InputSystem;

public class InputMechanic : MonoBehaviour
{
    public int maxMessage = 25;

    [SerializeField] // Corrected attribute typo
    List<Message> messageList = new List<Message>();

    //[SerializeField] // Assuming you want to reference the InputField in the inspector
    //InputField inputBox;

    // Start is called before the first frame update
    void Start()
    {
        // Initialization code here if needed
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SendMessageToChat("You pressed the enter key.");
            Debug.Log("enter is press");
        }

        /*if (!inputBox.isFocused && Input.GetKeyDown(KeyCode.Return))
        {
            inputBox.ActivateInputField();
        }*/
    }

    public void SendMessageToChat(string text)
    {
        if (messageList.Count >= maxMessage)
            messageList.Remove(messageList[0]);

        Message newMessage = new Message();
        newMessage.text = text;
        messageList.Add(newMessage);
    }
}

[System.Serializable] // Corrected attribute typo
public class Message
{
    public string text;
}
