using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddRemoveActiveContextOnCollision : MonoBehaviour
{
  public Context context;
  public GameManager gameManager;
    private void OnTriggerEnter(Collider other)
    {
        gameManager.GetComponent<GameManager>().currentContexts.Add(context);
    }
    private void OnTriggerExit(Collider other)
    {
        gameManager.GetComponent<GameManager>().currentContexts.Remove(context);
    }
}
