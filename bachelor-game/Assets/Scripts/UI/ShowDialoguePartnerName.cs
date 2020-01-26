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

    Text showPlayerInputHistory;
    Text showResponseHistory;
    CharacterDialogueInfos characterDialogueInfos;


    private void Start()
    {
        //showPlayerInputHistory = GetComponent<SendPlayerInput>().PlayerInputOutput.GetComponent<Text>();
        //showResponseHistory = GetComponent<SendPlayerInput>().AnswerText.GetComponent<Text>();
    }

    private void Update()
    {
        if (GetComponent<GameManager>().DialoguePartner != null)
        {
            showDialoguePartnerDialogueHistory();
        }
    }


    public void ChangeNameAndColor()
    {
        //change Name
        charName = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().characterName;
        dialoguePartner.GetComponent<Text>().text = charName;

        //change Color
        charColor = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().color;
        dialoguePartner.GetComponent<Text>().color = charColor;
        //dialoguePartnerAnswer.GetComponent<Text>().color = charColor;
    }

    public void showDialoguePartnerDialogueHistory()
    {
       // showPlayerInputHistory.text = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().playerInputHistory;
       // showResponseHistory.text = GetComponent<GameManager>().DialoguePartner.GetComponent<CharacterDialogueInfos>().responseHistory;
    }

}
