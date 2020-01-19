using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class OnSendMessage : MonoBehaviour
{
    public UnityEvent ExecuteMessage;

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            ExecuteMessage.Invoke();
        }
    }
}
