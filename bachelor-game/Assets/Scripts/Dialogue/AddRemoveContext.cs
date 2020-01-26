using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AddRemoveContext : MonoBehaviour
{
    private List<Context> listOfContexts;

    private void Start()
    {
        listOfContexts = GetComponent<DialogueTrigger>().context;
    }

    public void AddContext(Context C)
	{
        listOfContexts.Add(C);
	}

    public void RemoveContext(Context C)
    {
        listOfContexts.Remove(C);
    }
}
