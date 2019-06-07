using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    public float range = 100f;

    public Camera fpsCam;

    bool inDialogue = false;

    public GameObject interactionIcon;

    List<Context> contextBuffer;

    public GameObject gameManager;



    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (inDialogue == false) 
            { 
                if(hit.transform)
                interactionIcon.SetActive(true);

                if (Input.GetButtonDown("Interact"))
                {
                    //enter Dialogue
                    gameManager.GetComponent<GameManager>().DialoguePartner = hit.transform.gameObject;
                    contextBuffer = new List<Context>(hit.transform.GetComponent<DialogueTrigger>().context);

                    gameManager.GetComponent<GameManager>().currentContexts.AddRange(contextBuffer);

                    gameManager.GetComponent<EnterDialogue>().StartDialogue();

                    interactionIcon.SetActive(false);

                    transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;



                    inDialogue = true;
                }
            }

            Debug.Log(hit.transform.name);

        }
        else { interactionIcon.SetActive(false); }

        if (inDialogue == true)
        {
            if (Input.GetButtonDown("Escape"))
            {
                transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                gameManager.GetComponent<EnterDialogue>().EndDialogue();
                gameManager.GetComponent<GameManager>().DialoguePartner = null;
                for (int i = 0; i < contextBuffer.Count; i++)
                {
                    gameManager.GetComponent<GameManager>().currentContexts.Remove(contextBuffer[i]);
                }
                contextBuffer.Clear();



                inDialogue = false;
            }
        }

    }

  
}
