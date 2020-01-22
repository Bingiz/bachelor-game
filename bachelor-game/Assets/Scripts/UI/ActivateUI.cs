using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : MonoBehaviour
{
    public Canvas ui;
    public bool inDialogue = false;

    public void Start()
    {
        //ui.enabled = false;
    }

    // Update is called once per frame
    public void activateDialogueUI()
    {
        ui.enabled = true;
        inDialogue = true;
    }

    public void deactivateDialogueUI()
    {
        ui.enabled = false;
        inDialogue = false;
    }
}
