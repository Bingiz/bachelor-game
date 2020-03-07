using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddRemoveContext : MonoBehaviour
{
    public void AddContext(Context C)
	{
        //Debug.Log("Context: "+ C.name + " added to " + gameObject);
        //listOfContexts.Add(C);
        //GameObject.Find("GameManager").GetComponent<GameManager>().DialoguePartner.GetComponent<DialogueTrigger>().context.Add(C);
        GameObject.Find(this.name).GetComponent<DialogueTrigger>().context.Add(C);

    }

    public void RemoveContext(Context C)
    {
        //GameObject.Find("GameManager").GetComponent<GameManager>().DialoguePartner.GetComponent<DialogueTrigger>().context.Remove(C);
        GameObject.Find(this.name).GetComponent<DialogueTrigger>().context.Remove(C);
    }
}
