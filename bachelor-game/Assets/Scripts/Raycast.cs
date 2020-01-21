using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    public float range = 100f;

    public Camera fpsCam;

    bool interacting = false;

    bool cooldown = false;

    bool viewingInformation = false;

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
                cooldown = true;

                if (hit.collider.gameObject.tag == "NPC" | hit.collider.gameObject.tag == "ViewableObject" | hit.collider.gameObject.tag == "InteractableObject")
                {
                    interactionIcon.SetActive(true);

                    if (Input.GetButtonDown("Interact"))
                    {
                        interactingWith = hit.collider.gameObject;
                        Debug.Log(interactingWith);

                        if (interactingWith.tag == "NPC")
                        {
                            EnterInteraction();
                            interactingWith.GetComponent<EnterDialogue>().StartDialogue();
                        }
                        else if (interactingWith.tag == "ViewableObject")
                        {
                            EnterInteraction();
                            interactingWith.GetComponent<ViewObject>().EnterView();
                        }
                        else if (interactingWith.tag == "InteractableObject")
                        {
                            if (interactingWith.GetComponent<InteractableObject>().Interaction())
                            {
                                EnterInteraction();
                            }

                        }
                        else
                        {
                            interactingWith = null;
                        }
                    }
                }
            }
        }
        else { interactionIcon.SetActive(false); }

        if (interacting == true && cooldown == false)
        {

            if (Input.GetMouseButtonDown(0) && !viewingInformation && !cooldown)
            {
                cooldown = true;
                viewingInformation = true;
                interactingWith.GetComponent<ViewObject>().ViewInformation(true);
            }
            else if (Input.GetMouseButtonDown(0) && viewingInformation && !cooldown)
            {
                cooldown = true;
                viewingInformation = false;
                interactingWith.GetComponent<ViewObject>().ViewInformation(false);
            }

            cooldown = true;

            if (Input.GetButtonDown("Escape"))
            {


                if (interactingWith.tag == "NPC")
                {
                    interactingWith.GetComponent<EnterDialogue>().EndDialogue();
                }
                else if (interactingWith.tag == "ViewableObject")
                {
                    interactingWith.GetComponent<ViewObject>().QuitView();
                }
                interactingWith = null;
                interacting = false;
                transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            }

            if (Input.GetButtonDown("Interact"))
            {
                if (interactingWith.tag == "ViewableObject")
                {
                    transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                    if (interactingWith.GetComponent<ViewObject>().item != null)
                    {
                        if (interactingWith.GetComponent<ViewObject>().item.collectible == true)
                        {
                            //collect item
                            Debug.Log(interactingWith.GetComponent<ViewObject>().item.name + " collected.");
                            interactingWith.GetComponent<ViewObject>().CollectItem();
                        }
                    }
                    ExitInteraction();
                }
            }
        }

        cooldown = false;

    }

    void ExitInteraction()
    {
        interactingWith.GetComponent<ViewObject>().QuitView();
        interactingWith = null;
        interacting = false;
    }
    void EnterInteraction()
    {
        interactionIcon.SetActive(false);
        transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        interacting = true;
    }
    

  
}
