using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialogue : MonoBehaviour
{

    public GameObject inputFieldObj;


    public void StartDialogue()
    {
        GetComponent<ActivateUI>().activateDialogueUI();
        inputFieldObj.GetComponent<InputFieldAutoActivate>().activateField();
    }

    public void EndDialogue()
    {
        GetComponent<ActivateUI>().deactivateDialogueUI();
        inputFieldObj.GetComponent<InputFieldAutoActivate>().deactivateField();
    }
}
