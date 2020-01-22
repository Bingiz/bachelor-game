using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialogue : MonoBehaviour
{

    public GameObject inputFieldObj;

    public GameManager gameManager;
    DialogueTrigger dialogueTrigger;
    InputFieldAutoActivate inputFieldAutoActivate;
    ActivateUI activateUI;

    ShowDialoguePartnerName showDialoguePartnerName;

    Raycast raycast;

    List<Context> contextBuffer;

    private void Start()
    {
        
        showDialoguePartnerName = gameManager.gameObject.GetComponent<ShowDialoguePartnerName>();
        activateUI = gameManager.GetComponent<ActivateUI>();
        activateUI.deactivateDialogueUI();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        inputFieldAutoActivate = inputFieldObj.GetComponent<InputFieldAutoActivate>();
        raycast = GameObject.Find("FPSController/FirstPersonCharacter").GetComponent<Raycast>();
    }


    public void StartDialogue()
    {
        //enter Dialogue

        UpdateContexts();

        //change Name and Color to Dialogue Partners
        showDialoguePartnerName.ChangeNameAndColor();

        gameManager.GetComponent<SendPlayerInput>().Greeting();
        
        activateUI.activateDialogueUI();
        inputFieldAutoActivate.activateField();

        raycast.inDialogue = true;
    }

    public void UpdateContexts()
    {
        gameManager.DialoguePartner = this.gameObject;
        contextBuffer = new List<Context>(dialogueTrigger.context);

        gameManager.currentContexts.Clear();

        gameManager.currentContexts.AddRange(contextBuffer);
    }


    public void EndDialogue()
    {
        gameManager.DialoguePartner = null;
        for (int i = 0; i < contextBuffer.Count; i++)
        {
            gameManager.currentContexts.Remove(contextBuffer[i]);
        }

        contextBuffer.Clear();
        activateUI.deactivateDialogueUI();
        inputFieldAutoActivate.deactivateField();
        raycast.inDialogue = false;
    }
}
