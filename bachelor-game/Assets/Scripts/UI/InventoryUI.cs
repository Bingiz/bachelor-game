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
    public List<GameObject> ItemsInInventory;

    // Start is called before the first frame update
    void Start()
    {
        rt = GetComponent<RectTransform>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void UpdateInventoryUISize()
    {
        x = gameManager.itemsInInventory.Count * 100;
        rt.sizeDelta = new Vector2(x, y);
    }

    public void AddInventoryObject(Item item)
    {
        GameObject newItem = Instantiate(itemInUI, this.gameObject.transform.position, this.gameObject.transform.rotation);
        newItem.transform.SetParent(this.gameObject.transform, false);
        newItem.GetComponent<Image>().sprite = item.icon;
        newItem.name = item.name;
        UpdateInventoryUISize();
    }

    public void RemoveInventoryObject(Item item)
    {
        foreach (Transform child in this.gameObject.transform)
        {
            if (child.name == item.name)
            {
                Destroy(child);
                UpdateInventoryUISize();
            }
            Debug.Log("Foreach loop: " + child);
        }

    }
}
