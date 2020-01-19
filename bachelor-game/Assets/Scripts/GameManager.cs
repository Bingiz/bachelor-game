using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public List<Context> currentContexts;

    public List<Item> itemsInInventory;

    public InputTag currentTopic;

    //public List<InputTag> listOfAllInputTags;

    [HideInInspector] public StemDictionary stemDictionary;

    public GameObject DialoguePartner;

    private void Awake()
    {
        stemDictionary = GetComponent<StemDictionary>();
        currentContexts = new List<Context>();
    }
}
