using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : MonoBehaviour
{
    public GameObject ui;
    public bool inDialogue = false;

    public void Start()
    {
        //ui.enabled = false;
    }

    // Update is called once per frame
    public void activateDialogueUI()
    {
        ui.SetActive(true);
        inDialogue = true;
    }

    public void deactivateDialogueUI()
    {
        ui.SetActive(false);
        inDialogue = false;
        GetComponent<MessageManager>().ClearMessageList();
    }
}
