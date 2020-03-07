using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour
{
    bool paused = false;
    public Canvas pauseScreen, inventoryScreen;
    public Raycast raycast;

    private void Start()
    {
           
    }

    public void PauseResume()
    {
        if (paused)
        {
                //resume
                pauseScreen.enabled = false;
                Time.timeScale = 1;
                raycast.ExitInteraction();
                paused = !paused;


        }
        else
        {
            if (raycast.interactingWith != null)
            {

            }
            else
            {
                pauseScreen.enabled = true;
                Time.timeScale = 0;
                raycast.EnterInteraction();
                Cursor.lockState = CursorLockMode.None;
                //pause
                paused = !paused;
            }
        }
        
    }

    public void OpenCloseInventory()
    {

    }

    private void Update()
    {
        if (Input.GetButtonDown("Escape"))
        {
            PauseResume();
        }

        if (Input.GetButtonDown("Tab"))
        {
            OpenCloseInventory();
        }
    }
}
