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

    List<Context> contextBuffer;

    private void Start()
    {
        showDialoguePartnerName = gameManager.gameObject.GetComponent<ShowDialoguePartnerName>();
        activateUI = gameManager.GetComponent<ActivateUI>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
        inputFieldAutoActivate = inputFieldObj.GetComponent<InputFieldAutoActivate>();
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
    }
}
