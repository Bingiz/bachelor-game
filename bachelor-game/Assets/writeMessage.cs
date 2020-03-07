using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class writeMessage : MonoBehaviour
{
    public Slide_Info slide_Info;

    private void Start()
    {
        slide_Info.ShowMessage("Mit der TAB Taste, kannst du jederzeit die Crewliste ein und ausblenden.");
        Destroy(this.gameObject);
    }
}
