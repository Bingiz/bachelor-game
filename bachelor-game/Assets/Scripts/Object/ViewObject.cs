using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ViewObject : MonoBehaviour
{
    Vector3 originalPosition;
    Quaternion originalRotation;
    GameManager gameManager;
    GameObject offset;
    GameObject itemTitle;
    GameObject itemDescription;
    GameObject itemInfoUI;
    GameObject inventory;
    //private bool viewingInformation = false;

    public Item item;

    private void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        offset = GameObject.Find("Offset");
        itemInfoUI = GameObject.Find("/UI/ItemDescription/Background");
        itemInfoUI.SetActive(false);
        inventory = GameObject.Find("/UI/Inventory/Background");
    }




    private void Update()
    {
        /*
        if (Input.GetMouseButtonDown(0))
        {
            ViewInformation(!viewingInformation);
            viewingInformation = !viewingInformation;
            
        }
        */
    }

    public void ViewInformation(bool yn)
    {
        if (item != null)
        {
            if (yn)
            {
                itemInfoUI.SetActive(true);

                itemTitle = GameObject.Find("/UI/ItemDescription/Background/Title");
                itemDescription = GameObject.Find("/UI/ItemDescription/Background/Text");
                itemTitle.GetComponent<Text>().text = item.name;
                itemDescription.GetComponent<Text>().text = item.description;
            }
            else
            {
                itemInfoUI.SetActive(false);
            }
        }
    }

    public void EnterView()
    {
        Debug.Log("View Entered");

        this.transform.position = offset.transform.position;

        GetComponent<ViewObject>().enabled = true;
    }
    public void QuitView()
    {
        Debug.Log("View Ended");
        GetComponent<ViewObject>().enabled = false;
        this.transform.position = originalPosition;
        this.transform.rotation = originalRotation;
    }

    public void CollectItem()
    {
        gameManager.itemsInInventory.Add(item);
        inventory.GetComponent<InventoryUI>().AddInventoryObject(item);
        Destroy(this.gameObject);
    }

}
