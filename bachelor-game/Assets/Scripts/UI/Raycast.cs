using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycast : MonoBehaviour
{

    public float range = 100f;

    public Camera fpsCam;

    bool interacting = false;

    public GameObject interactionIcon;

    public bool inDialogue = false;

    bool cooldown = false;
    bool viewingInformation = false;

    public GameObject interactingWith;

    public GameObject viewOverlay;



    // Update is called once per frame
    void Update()
    {
        /*
        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (hit.collider.gameObject.tag == "interactable")
            {
                interactionIcon.SetActive(!interacting);
                if (Input.GetButtonDown("Interact") && !inDialogue)
                {
                    interactingWith = hit.collider.gameObject.GetComponent<OpenUI>();
                    Debug.Log(hit.collider.gameObject.GetComponent<OpenUI>());

                    if (interactingWith.interactable)
                    {
                        interactingWith.ShowUIElement();
                    }
                    if (!interacting)
                    {
                        EnterInteraction();
                    }
                    else
                    {
                        ExitInteraction();
                    }
                }

            }
        }
        else
        {
            interactionIcon.SetActive(false);
        }
        */
        if (interactingWith != null)
        {
            if (interactingWith.tag == "ViewableObject")
            {
                viewOverlay.active = true;
            }
        }
        else
        {
            viewOverlay.active = false;
        }



        RaycastHit hit;
        if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
        {
            if (interacting == false)
            {
                cooldown = true;

                if (hit.collider.gameObject.tag == "NPC" | hit.collider.gameObject.tag == "ViewableObject" | (hit.collider.gameObject.tag == "InteractableObject" && hit.collider.gameObject.GetComponent<InteractableObject>().solved == false))
                {
                    interactionIcon.SetActive(true);

                    if (Input.GetButtonDown("Interact"))
                    {
                        interactingWith = hit.collider.gameObject;
                        //Debug.Log(interactingWith);

                        if (interactingWith.tag == "NPC")
                        {
                            EnterInteraction();
                            Cursor.lockState = CursorLockMode.None;
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
                                    Cursor.lockState = CursorLockMode.None;
                                    EnterInteraction();
                                    //show Lock
                                    interactingWith.GetComponent<InteractableObject>().openCodeUI(true);

                            }

                        }
                        else
                        {
                           // interactingWith = null;
                        }
                    }
                }
            }
        }
        else { interactionIcon.SetActive(false); }

        if (interacting == true && cooldown == false && interactingWith != null)
        {
            if (interactingWith.tag =="ViewableObject")
            {
                if (Input.GetMouseButtonDown(0) && !cooldown && interacting)
                {
                    cooldown = true;
                    interactingWith.GetComponent<ViewObject>().ViewInformation(!viewingInformation);
                    viewingInformation = !viewingInformation;
                    
                }

            }


            //cooldown = true;
            /*
            if (Input.GetButtonDown("Escape"))
            {


                if (interactingWith.tag == "NPC")
                {
                    interactingWith.GetComponent<EnterDialogue>().EndDialogue();
                }
                else if (interactingWith.tag == "ViewableObject")
                {
                    interactingWith.GetComponent<ViewObject>().QuitView();
                    interactingWith.GetComponent<ViewObject>().ViewInformation(false);
                }
                interactingWith = null;
                interacting = false;
                transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            }
            */

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
                    Debug.Log("Exits interaction");
                    interactingWith.GetComponent<ViewObject>().ViewInformation(false);
                    interactingWith.GetComponent<ViewObject>().QuitView();
                    ExitInteraction(); 
                }
                else if (interactingWith.tag == "InteractableObject")
                {
                    interactingWith.GetComponent<InteractableObject>().openCodeUI(false);
                    ExitInteraction();
                }
            }
        }

        cooldown = false;

    }

    public void ExitInteraction()
    {
        if (interactingWith != null)
        {
            interactingWith = null;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        //interactingWith.GetComponent<ViewObject>().QuitView();
        GameObject controller = GameObject.Find("FPSController");
        controller.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        interacting = false;
        
    }
    public void EnterInteraction()
    {
        Cursor.visible = true;
        interactionIcon.SetActive(false);
        transform.parent.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        interacting = true;
    }


}
