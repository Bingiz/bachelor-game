using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateCrewlist : MonoBehaviour
{
    bool active = true;
    public GameObject crewlist;
    public GameManager gameManager;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            active = !active;
            crewlist.SetActive(active);
        }
    }
}
