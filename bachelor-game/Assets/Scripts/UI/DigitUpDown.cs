using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DigitUpDown : MonoBehaviour
{
    public int digit;
    public int val;
    public Code code;

    public void Start()
    {
        Cursor.visible = true;
        code = GameObject.Find("UI/KeyCode").GetComponent<Code>();
        //Debug.Log(code);
    }
    public void changeDigit()
    {
        //Debug.Log("Digit " + digit + " is set " + val);
        code.ChangeDigit(digit -1, val);
    }
}
