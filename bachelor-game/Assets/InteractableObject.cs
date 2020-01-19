using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{

    public bool needsItem;

    GameManager gameManager;

    public Item neededItem;

    public UnityEvent DoOnInteraction;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public bool Interaction()
    {
        if (needsItem)
        {
            if (gameManager.itemsInInventory.Contains(neededItem))
            {
                Debug.Log(neededItem.name + " used successfully!");
                DoOnInteraction.Invoke();
                return true;
            }
            else
            {
                Debug.Log("You need " + neededItem.name + " to operate this.");
                return false;
            }
        }
        else
        {
            DoOnInteraction.Invoke();
            Debug.Log("Used successfully!");
            return true;
        }
    }
}
