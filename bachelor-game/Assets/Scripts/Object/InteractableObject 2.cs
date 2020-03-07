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

    GameObject infoBox;

    Transform inventory;
    public GameObject codeUI;
    Code code;

    private void Start()
    {
        inventoryUI = GameObject.Find("UI/Inventory/Background").GetComponent<InventoryUI>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("UI/Inventory/Background").GetComponent<Transform>();
        infoBox = GameObject.Find("UI/Info/Background");
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

    public void WrongNumber()
    {
        infoBox.GetComponent<Slide_Info>().ShowMessage("Diese Einstellung scheint nicht zu funktionieren.");
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
                //infoBox.GetComponent<Slide_Info>().ShowMessage("Du benötigst ein "+ neededItem.name + " um diese Maschine zu benutzen.");
                infoBox.GetComponent<Slide_Info>().ShowMessage("Du benötigst ein Ersatzteil um diese Maschine aktivieren zu können.");
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
