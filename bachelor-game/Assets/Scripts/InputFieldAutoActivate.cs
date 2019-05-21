using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldAutoActivate : MonoBehaviour
{
    //public InputField inputField = GetComponent<InputField>();
    public void Start()
    {
        GetComponent<InputField>().ActivateInputField();
    }

    void Update()
    {
        GetComponent<InputField>().ActivateInputField();
    }

    public void clearInputfield()
    {
        GetComponent<InputField>().text = null;
    }
}
