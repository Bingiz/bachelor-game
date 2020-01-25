using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class InteractableObject : MonoBehaviour
{

    public bool needsItem;
    public bool solved = false;

    GameManager gameManager;

    public Item neededItem;

    public UnityEvent DoOnInteraction;

    public UnityEvent DoOnSolve;

    InventoryUI inventoryUI;

    Transform inventory;
    public GameObject codeUI;
    Code code;

    private void Start()
    {
        inventoryUI = GameObject.Find("UI/Inventory/Background").GetComponent<InventoryUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("UI/Inventory/Background").GetComponent<Transform>();
    }

    public void openCodeUI(bool open)
    {
        codeUI.SetActive(open);
    }

    public void Solved()
    {
        solved = true;
        Debug.Log("SOLVED");
        DoOnSolve.Invoke();
    }

    public bool Interaction()
    {
        if (needsItem)
        {
            if (gameManager.itemsInInventory.Contains(neededItem))
            {
                Debug.Log(neededItem.name + " used successfully!");
                DoOnInteraction.Invoke();

                gameManager.itemsInInventory.Remove(neededItem);

                foreach (Transform item in inventory)
                {
                    if (item.name == neededItem.name)
                    {
                        Destroy(item.gameObject);
                        break;
                    }
                }

                inventoryUI.UpdateInventoryUISize();

                needsItem = false;
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
