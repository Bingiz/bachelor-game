using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    public float range = 100f;

    public Camera fpsCam;

    bool interacting = false;

    public GameObject interactionIcon;


    GameObject interactingWith;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {   

            if (interacting == false) 
            {
                if (hit.collider.gameObject.tag == "NPC" | hit.collider.gameObject.tag == "ViewableObject")
                {
                    interactionIcon.SetActive(true);
                }

                if (Input.GetButtonDown("Interact"))
                {


                    interactingWith = hit.collider.gameObject;

                    if (interactingWith.tag == "NPC")
                    {
                        interactingWith.GetComponent<EnterDialogue>().StartDialogue();
                    }
                    else if (interactingWith.tag == "ViewableObject")
                    {
                        interactingWith.GetComponent<ViewObject>().EnterView();
                    }


                    interactionIcon.SetActive(false);

                    transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;



                    interacting = true;
                }
            }

        }
        else { interactionIcon.SetActive(false); }

        if (interacting == true)
        {
            if (Input.GetButtonDown("Escape"))
            {
                transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                if (interactingWith.tag == "NPC")
                {
                    interactingWith.GetComponent<EnterDialogue>().EndDialogue();
                }
                  else if (interactingWith.tag == "ViewableObject")
                  {
                      interactingWith.GetComponent<ViewObject>().QuitView();
                  }
                  

                interacting = false;
            }
        }

    }

  
}
