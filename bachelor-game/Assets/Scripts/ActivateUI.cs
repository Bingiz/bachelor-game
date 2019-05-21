using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActivateUI : MonoBehaviour
{
    public Canvas ui;

    // Update is called once per frame
    public void activateDialogueUI()
    {
        ui.enabled = !ui.enabled;
    }
}
