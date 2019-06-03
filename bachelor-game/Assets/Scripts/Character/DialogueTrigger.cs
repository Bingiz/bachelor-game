using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class DialogueTrigger : MonoBehaviour
{
    public GameManager gameManager;
    public List<Context> context;
    public UnityEvent EnterDialogue;
   

        void OnTriggerEnter(Collider other)
        {
            gameManager.DialoguePartner = this.gameObject;
            gameManager.GetComponent<GameManager>().currentContexts.AddRange(context);
        }

        void OnTriggerExit(Collider other)
        {
            gameManager.DialoguePartner = null;
            for (int i = 0; i < context.Count; i++)
            {
                gameManager.GetComponent<GameManager>().currentContexts.Remove(context[i]);
            }

        }
   
}
