using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnterDialogue : MonoBehaviour
{

    public GameObject inputFieldObj;

    public GameManager gameManager;
    DialogueTrigger dialogueTrigger;
    InputFieldAutoActivate inputFieldAutoActivate;
    ActivateUI activateUI;
    public Scrollbar scrollbar;

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
        //scrollbar = GameObject.Find("UI/DialogueUI/DialogueBox/Text Box/Scrollbar Vertical/").GetComponent<Scrollbar>();
    }


    public void StartDialogue()
    {
        //enter Dialogue

        UpdateContexts();

        //change Name and Color to Dialogue Partners
        showDialoguePartnerName.ChangeNameAndColor();
        gameManager.GetComponent<SendPlayerInput>().DialoguePartner = gameManager.DialoguePartner;

        gameManager.GetComponent<SendPlayerInput>().Greeting();
        
        activateUI.activateDialogueUI();
        inputFieldAutoActivate.activateField();

        raycast.inDialogue = true;

        scrollbar.value = 0;
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

        gameManager.GetComponent<SendPlayerInput>().DialoguePartner = null;

        contextBuffer.Clear();
        activateUI.deactivateDialogueUI();
        inputFieldAutoActivate.deactivateField();
        raycast.inDialogue = false;
        //gameManager.GetComponent<MessageManager>().ClearMessageList();
    }
}
