using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OpenUI : MonoBehaviour
{
    public bool interactable;
    public GameObject UIElement;
    private bool UIOpen = false;
    public UnityEvent doOnInteraction;
    public UnityEvent doOnExitInteraction;

    public void ShowUIElement()
    {
        UIElement.SetActive(!UIOpen);
        UIOpen = !UIOpen;
        if (UIOpen)
        {
            doOnInteraction.Invoke();
        }
        else
        {
            doOnExitInteraction.Invoke();
        }
        
    }
}
