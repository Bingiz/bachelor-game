using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class activateBoxContent : MonoBehaviour
{
    public GameObject elektronenröhre;


    public void activateContent()
    {
        elektronenröhre.active = true;
    }

    public void openBox()
    {
        GetComponent<Animator>().SetTrigger("Open");
    }
}
