using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowDialoguePartnerName : MonoBehaviour
{

    public GameObject dialoguePartner;
    public GameObject dialoguePartnerAnswer;
    string charName;
    Color charColor;
  


    private void Update()
    {
        //on dialogue enter
        ChangeNameAndColor();
        showDialoguePartnerDialogueHistory();



    }

    public void ChangeNameAndColor()
    {
        charName = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().characterName;
        dialoguePartner.GetComponent<Text>().text = charName;
        //Debug.Log("Name Changed");
        charColor = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().color;
        dialoguePartner.GetComponent<Text>().color = charColor;
        dialoguePartnerAnswer.GetComponent<Text>().color = charColor;
        //Debug.Log("Color Changed");
    }

    public void showDialoguePartnerDialogueHistory()
    {
        GetComponent<SendPlayerInput>().PlayerInputOutput.GetComponent<Text>().text = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().playerInputHistory;
        GetComponent<SendPlayerInput>().AnswerText.GetComponent<Text>().text = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().responseHistory;
    }
}
