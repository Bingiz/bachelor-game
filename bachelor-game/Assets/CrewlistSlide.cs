using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrewlistSlide : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<Animator>().SetBool("active", true);
    }
    private void OnDisable()
    {
        GetComponent<Animator>().SetBool("active", false);
    }
}
