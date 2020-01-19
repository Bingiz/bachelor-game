using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    GameManager gameManager;
    public GameObject itemInUI;
    int x = 0;
    int y = 100;
    RectTransform rt;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void UpdateInventoryUISize()
    {
        x = gameManager.itemsInInventory.Count * 100;
        rt.sizeDelta = new Vector2(x, y);
    }

    public void AddInventoryObject(Item item)
    {
        GameObject newItem = Instantiate(itemInUI,this.gameObject.transform.position,this.gameObject.transform.rotation);
        newItem.transform.SetParent(this.gameObject.transform,false);
        newItem.GetComponent<Image>().sprite = item.icon;
        UpdateInventoryUISize();
    }
}
