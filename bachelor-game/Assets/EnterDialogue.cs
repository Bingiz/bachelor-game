using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterDialogue : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(GetComponent<GameManager>().DialoguePartner != null)
            {
                StartDialogue();
            }
        }
    }

    void StartDialogue()
    {
        GetComponent<ActivateUI>().activateDialogueUI();
    }
}
