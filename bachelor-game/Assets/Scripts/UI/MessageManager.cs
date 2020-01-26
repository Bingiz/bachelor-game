using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    //https://www.youtube.com/watch?v=IRAeJgGkjHk tutorial to this

    public int maxMessages = 25;

    public GameObject chatPanel, textObject;
    public InputField chatBox;
    public Color playerCol, adelaideCol, edgarCol, lawrenceCol;

    List<Message> messageList = new List<Message>();

    private void Update()
    {
        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SendMessageToChat("You pressed the space bar!", Message.MessageType.playerMessage);
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType)
    {
        if (messageList.Count >= maxMessages)
        {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }


    public void ClearMessageList()
    {
        foreach (Message message in messageList)
        {
            Destroy(message.textObject.gameObject);
            
        }
        messageList.Clear();
    }

    public Color MessageTypeColor(Message.MessageType messageType)
    {
        Color color = playerCol;

        switch (messageType)
        {
            case Message.MessageType.adelaide:
                color = adelaideCol;
                break;

            case Message.MessageType.edgar:
                color = edgarCol;
                break;

            case Message.MessageType.lawrence:
                color = lawrenceCol;
                break;

            default:
                color = playerCol;
                break;
        }

        return color;
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