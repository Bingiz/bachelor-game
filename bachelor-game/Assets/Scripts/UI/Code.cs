using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Code : MonoBehaviour
{
    public int[] requiredCode;
    public int[] inputCode;
    public Text codeText;

    Raycast raycast;

    private void Awake()
    {
        raycast = GameObject.Find("FPSController/FirstPersonCharacter").GetComponent<Raycast>();
        Debug.Log(this);
        requiredCode = new int[4];
        inputCode = new int[4] { 0, 0, 0, 0 };
        DisplayDigits();
    }

    public void ChangeDigit(int digit, int val)
    {
        inputCode[digit] += val;
        if (inputCode[digit] < 0)
        {
            inputCode[digit] = 9;
        }
        else if (inputCode[digit] > 9)
        {
            inputCode[digit] = 0;
        }

        DisplayDigits();

        //SendLockInformation();
    }

    private void OnEnable()
    {
        GetLockInformation();
        SetLockInformation();
    }

    public void GetLockInformation()
    {
        if (raycast.interactingWith != null)
        {
            inputCode = raycast.interactingWith.GetComponent<CodeObject>().inputCode;
            requiredCode = raycast.interactingWith.GetComponent<CodeObject>().requiredCode;
        }
        DisplayDigits();
    }
    public void SetLockInformation()
    {
        raycast.interactingWith.GetComponent<CodeObject>().inputCode = inputCode;
    }

    private void DisplayDigits()
    {
        codeText.text = inputCode[0].ToString() + " " + inputCode[1].ToString() + " " + inputCode[2].ToString() + " " + inputCode[3].ToString();
    }

    public void CheckCode()
    {
        bool codeIsRight = true;
        for (int i = 0; i < requiredCode.Length; i++)
        {
                if (requiredCode[i] != inputCode[i])
                {
                    //Debug.Log("Checking: " + requiredCode[i] + " against " + inputCode[i]);
                    codeIsRight = false;
                    break;
                }
        }

        if (codeIsRight)
        {
            Debug.Log("Code is right!");
            raycast.interactingWith.GetComponent<InteractableObject>().Solved();
            raycast.interactingWith.GetComponent<InteractableObject>().openCodeUI(false);
            raycast.ExitInteraction();
        }
        else
        {
            Debug.Log("Code is wrong!");
        }
    }
}
