using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewObject : MonoBehaviour
{

    Vector3 originalPosition;
    Quaternion originalRotation;
    GameObject offset;

    private void Start()
    {
        originalPosition = this.transform.position;
        originalRotation = this.transform.rotation;
        offset = GameObject.Find("Offset");
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
}
