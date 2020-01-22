using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDialogueButton : MonoBehaviour
{
    Raycast raycast;

    // Start is called before the first frame update
    void Start()
    {
        raycast = GameObject.Find("FPSController/FirstPersonCharacter").GetComponent<Raycast>();

    }
    public void UI()
    {
        //raycast.interactingWith.ShowUIElement();
    }
}
