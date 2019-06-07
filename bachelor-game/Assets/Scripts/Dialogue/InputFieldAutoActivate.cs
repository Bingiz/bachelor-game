using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldAutoActivate : MonoBehaviour
{
    //public InputField inputField = GetComponent<InputField>();
    public void Start()
    {
        //GetComponent<InputField>().ActivateInputField();
    }

    public void activateField()
    {
        GetComponent<InputField>().ActivateInputField();
    }

    public void deactivateField()
    {
        GetComponent<InputField>().DeactivateInputField();
    }

    public void clearInputfield()
    {
        GetComponent<InputField>().text = null;
    }
}
