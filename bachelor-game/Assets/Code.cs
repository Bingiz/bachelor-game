using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Code : MonoBehaviour
{
    public int[] requiredCode;
    public int[] inputCode;
    public Text codeText;

    public UnityEvent DoOnSolve;

    private void Awake()
    {
        requiredCode = new int[4];
        inputCode = new int[4] {0,0,0,0};
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

    void GetLockInformation(int[] req , int[] input)
    {
        requiredCode = req;
        inputCode = input;
        DisplayDigits();
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
            for (int j = 0; j < inputCode.Length; j++)
            {
                if (requiredCode[i] != inputCode[j])
                {
                    codeIsRight = false;
                    break;
                }
            } 
        }

        if (codeIsRight)
        {
            Debug.Log("Code is right!");
            DoOnSolve.Invoke();
        }
        else
        {
            Debug.Log("Code is wrong!");
        }
    }
}
