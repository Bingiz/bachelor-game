using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=IRAeJgGkjHk tutorial to this

    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public list<Message> messageList = new list<Message>();

    public void SendMessageToChat (string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
        }
        Message newMessage = newMessage();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;

        messageList.Add(newMessage);        
    }
}

public class Message
{
    public string text;
    public Text textObject;
    public MessageType messageType;

    public enum MessageType
    {
        playerMessage,
        adelaide,
        edgar,
        lawrence
    }
}